/**
 * @brief �p�j���
 * @author ����
 * 
 * @details �t�߂̃����_���ʒu�܂ňړ� @n
 *          �ړ����I�������Idle��ԂɑJ��
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateWander : IEnemyState
{
    private float _randomRange = 20f;
    private float _angleSpeedMag = 0.5f;
    private float _speedMag = 0.25f;
    private Vector3 _nextPos = Vector3.zero;

    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateWander(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {
        _nextPos = NextRandomPos();
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
        Vector3 nextPos;

        nextPos = new Vector3(_enemy.transform.position.x + Random.Range(-_randomRange, _randomRange), _enemy.transform.position.y,
                                _enemy.transform.position.z + Random.Range(-_randomRange, _randomRange));

        return nextPos;
    }
}
