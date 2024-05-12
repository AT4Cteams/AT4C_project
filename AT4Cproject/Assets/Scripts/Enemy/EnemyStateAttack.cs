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

    Enemy _enemy;

    public EnemyStateAttack(Enemy enemy) => _enemy = enemy;
    public override void Entry()
    {
        _exitTime = 1f;

        _enemy.Attack();

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
