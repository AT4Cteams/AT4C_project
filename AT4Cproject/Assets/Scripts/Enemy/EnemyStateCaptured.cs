/**
 * @brief �ߊl
 * @author ����
 * 
 * @details �v���C���[�̕����Ɍ���
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateCaptured : IEnemyState
{

    Enemy _enemy;

    public EnemyState _state => EnemyState.Captured;
    public EnemyStateCaptured(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {
        //_enemy.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        _enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    public override void Update() 
    {
    }
    public override void Exit()
    {

    }
}
