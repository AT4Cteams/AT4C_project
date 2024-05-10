/**
 * @brief UŒ‚ó‘Ô
 * @author ‘ºã
 * 
 * @details 
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateAttack : IEnemyState
{
    private float _exitTime = 1f;
    private float _minTime = 0.5f;
    private float _maxTime = 2.0f;

    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateAttack(Enemy enemy) => _enemy = enemy;
    public override void Entry()
    {
        _exitTime = 1f;
        _enemy.EffectTest();

        _enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public override void Update() 
    {
        _exitTime -= Time.deltaTime;
        if( _exitTime < 0f ) 
        {
            _enemy.ChangeState(EnemyState.Idle);
        }
    }
    public override void Exit()
    {
        _enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
