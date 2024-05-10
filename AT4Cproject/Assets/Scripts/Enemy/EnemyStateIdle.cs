/**
 * @brief �ҋ@���
 * @author ����
 * 
 * @details �s��
 *          Enemy�N���X�ŉ������m����܂őҋ@
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateIdle : IEnemyState
{
    private float _exitTime = 0f;
    private float _minTime = 0.5f;
    private float _maxTime = 2.0f;

    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateIdle(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {
        _enemy.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        _exitTime = Random.Range(_minTime, _maxTime);

        _enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public override void Update() 
    {
        _exitTime -= Time.deltaTime;
        if( _exitTime < 0f ) 
        {
            _enemy.ChangeState(EnemyState.Wander);
        }
    }
    public override void Exit()
    {
        _enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
