/**
 * @brief �ҋ@���
 * @author ����
 * 
 * @details Enemy�N���X�ŉ������m����܂őҋ@
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateIdle : IEnemyState
{
    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateIdle(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {

    }
    public override void Update() 
    {

    }
    public override void Exit() 
    {
    
    }

    //// �������߂�������
    //private void SearchPlayer()
    //{
    //    Vector3 targetPos = _enemy._targetPos;
    //    Vector3 enemyPos = _enemy.transform.position;

    //    float distance = Vector3.Distance(targetPos, enemyPos);

    //    if (distance < _enemy._searchLength)
    //    {
    //        _enemy.ChangeState(EnemyState.Tracking);
    //        Debug.Log("�͂�����I");
    //    }
    //}
}
