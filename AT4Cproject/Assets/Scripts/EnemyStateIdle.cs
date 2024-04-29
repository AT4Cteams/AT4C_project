/**
 * @brief 待機状態
 * @author 村上
 * 
 * @details Enemyクラスで音を感知するまで待機
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

    //// 距離が近かったら
    //private void SearchPlayer()
    //{
    //    Vector3 targetPos = _enemy._targetPos;
    //    Vector3 enemyPos = _enemy.transform.position;

    //    float distance = Vector3.Distance(targetPos, enemyPos);

    //    if (distance < _enemy._searchLength)
    //    {
    //        _enemy.ChangeState(EnemyState.Tracking);
    //        Debug.Log("はっけん！");
    //    }
    //}
}
