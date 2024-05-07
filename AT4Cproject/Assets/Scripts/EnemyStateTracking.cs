/**
 * @brief �ǔ����
 * @author ����
 * 
 * @details Enemy�N���X���ŃZ�b�g����targetPos�܂ňړ� @n
 *          �ړ����I�������ʂ̏�ԂɑJ��
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateTracking : IEnemyState
{
    Enemy _enemy;

    public EnemyState _state => EnemyState.Tracking;
    public EnemyStateTracking(Enemy enemy) => _enemy = enemy;

    // �����������ǂ���
    private bool _isArrived;

    public override void Entry()
    {
        _isArrived = false;
    }
    public override void Update()
    {
        // tagetpos�܂ňړ�
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
        _enemy.transform.rotation = Quaternion.RotateTowards(_enemy.transform.rotation, targetRotation, _enemy.angularSpeed * Time.deltaTime);

        // �����̌����Ǝ��̈ʒu�̊p�x����30�x�ȏ�̏ꍇ�A���̏�Ő���
        float angle = Vector3.Angle(targetDir, _enemy.transform.forward);
        if (angle < 30f)
        {
            _enemy.transform.position += _enemy.transform.forward * _enemy.speed * Time.deltaTime;
            // �������̏ꍇ�̕␳
            //if (Vector3.Distance(nextPoint, transform.position) < 0.5f) transform.position = nextPoint;
        }

        // target�Ɍ������Ĉړ�
        _enemy.agent.SetDestination(_enemy.targetPos);
        _enemy.agent.nextPosition = _enemy.transform.position;

        // �ڕW�ʒu�ɓ��B�������ǂ������m�F
        if (!_isArrived && _enemy.agent.remainingDistance <= _enemy.agent.stoppingDistance)
        { 
            Debug.Log("�Ƃ����Ⴍ�I");

            _isArrived = true;

            _enemy.EffectTest();

            _enemy.targetVolume = 0f;

            _enemy.ChangeState(EnemyState.Wander);
        }
    }
}
