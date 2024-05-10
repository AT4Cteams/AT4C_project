/**
 * @brief �p�j���
 * @author ����
 * 
 * @details �t�߂̃����_���ʒu�܂ňړ� @n
 *          �ړ����I�������Idle��ԂɑJ��
 */

using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class EnemyStateWander : IEnemyState
{
    private float _randomRange = 20f;
    private float _angleSpeedMag = 0.5f;
    private float _speedMag = 0.5f;
    private Vector3 _nextPos = Vector3.zero;
    private Vector3 _prevPos = Vector3.zero;

    private int _randomCount = 0;

    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateWander(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {
        _nextPos = NextRandomPos();
        _prevPos = _nextPos;

        _enemy.agent.SetDestination(_nextPos);
    }
    public override void Update()
    {
        Move();
    }
    public override void Exit() 
    {
    
    }

    private void Move()
    {
        // ���Ɉړ�����ʒu
        var nextPos = _enemy.agent.steeringTarget;
        Vector3 targetDir = nextPos - _enemy.transform.position;

        // ����
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        _enemy.transform.rotation = Quaternion.RotateTowards(_enemy.transform.rotation, targetRotation, _enemy.angularSpeed * Time.deltaTime * _angleSpeedMag);

        // �����̌����Ǝ��̈ʒu�̊p�x����30�x�ȏ�̏ꍇ�A���̏�Ő���
        float angle = Vector3.Angle(targetDir, _enemy.transform.forward);
        if (angle < 30f)
        {
            _enemy.transform.position += _enemy.transform.forward * _enemy.speed * Time.deltaTime * _speedMag;
        }

        // target�Ɍ������Ĉړ�
        _enemy.agent.SetDestination(_nextPos);
        _enemy.agent.nextPosition = _enemy.transform.position;

        // ����
        if (_enemy.agent.remainingDistance <= _enemy.agent.stoppingDistance)
        {
            _enemy.ChangeState(EnemyState.Idle);
        }
    }

    private Vector3 NextRandomPos()
    {
        Vector3 nextPos = Vector3.zero;

        while (true)
        {
            Vector2 randValue1 = new Vector2(Random.Range(_randomRange / 2, _randomRange), Random.Range(_randomRange / 2, _randomRange));
            Vector2 randValue2 = new Vector2(Random.Range(-_randomRange / 2, -_randomRange), Random.Range(-_randomRange / 2, -_randomRange));

            int n = Random.Range(0, 2);
            int m = Random.Range(0, 2);

            Vector2 randValue;
            randValue.x = (n == 0) ? randValue1.x : randValue2.x;
            randValue.y = (m == 0) ? randValue1.y : randValue2.y;

            nextPos = new Vector3(_enemy.transform.position.x + randValue.x, _enemy.transform.position.y,
                                    _enemy.transform.position.z + randValue.y);

            if (IsInNavMeshBounds(nextPos))
            {
                _randomCount = 0;
                break;
            }
            else
            {
                _randomCount++;
                if(_randomCount > 10)
                {
                    _randomCount = 0;
                    nextPos = _prevPos;
                    return nextPos;
                }
            }
        }
        return nextPos;
    }

    // �w�肳�ꂽ�ʒu��NavMesh�͈͓̔��ɂ��邩�ǂ����𔻒f���郁�\�b�h
    private bool IsInNavMeshBounds(Vector3 point)
    {
        NavMeshHit hit;
        // NavMesh��̓_���T���v�����O���āA���ꂪ���s�\���ǂ������m�F����
        return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
    }
}
