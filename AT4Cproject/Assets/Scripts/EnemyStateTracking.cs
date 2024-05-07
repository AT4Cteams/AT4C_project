/**
 * @brief 追尾状態
 * @author 村上
 * 
 * @details Enemyクラス側でセットしたtargetPosまで移動 @n
 *          移動が終わったら別の状態に遷移
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

    // 到着したかどうか
    private bool _isArrived;

    public override void Entry()
    {
        _isArrived = false;
    }
    public override void Update()
    {
        // tagetposまで移動
        Move();
    }


    public override void Exit()
    {

    }

    private void Move()
    {
        // 次に移動する位置
        var nextPos = _enemy.agent.steeringTarget;
        Vector3 targetDir = nextPos - _enemy.transform.position;

        // 旋回
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        _enemy.transform.rotation = Quaternion.RotateTowards(_enemy.transform.rotation, targetRotation, _enemy.angularSpeed * Time.deltaTime);

        // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
        float angle = Vector3.Angle(targetDir, _enemy.transform.forward);
        if (angle < 30f)
        {
            _enemy.transform.position += _enemy.transform.forward * _enemy.speed * Time.deltaTime;
            // もしもの場合の補正
            //if (Vector3.Distance(nextPoint, transform.position) < 0.5f) transform.position = nextPoint;
        }

        // targetに向かって移動
        _enemy.agent.SetDestination(_enemy.targetPos);
        _enemy.agent.nextPosition = _enemy.transform.position;

        // 目標位置に到達したかどうかを確認
        if (!_isArrived && _enemy.agent.remainingDistance <= _enemy.agent.stoppingDistance)
        { 
            Debug.Log("とうちゃく！");

            _isArrived = true;

            _enemy.EffectTest();

            _enemy.targetVolume = 0f;

            _enemy.ChangeState(EnemyState.Wander);
        }
    }
}
