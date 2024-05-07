/**
 * @brief 徘徊状態
 * @author 村上
 * 
 * @details 付近のランダム位置まで移動 @n
 *          移動が終わったらIdle状態に遷移
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
        // 次に移動する位置
        var nextPos = _enemy.agent.steeringTarget;
        Vector3 targetDir = nextPos - _enemy.transform.position;

        // 旋回
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        _enemy.transform.rotation = Quaternion.RotateTowards(_enemy.transform.rotation, targetRotation, _enemy.angularSpeed * Time.deltaTime * _angleSpeedMag);

        // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
        float angle = Vector3.Angle(targetDir, _enemy.transform.forward);
        if (angle < 30f)
        {
            _enemy.transform.position += _enemy.transform.forward * _enemy.speed * Time.deltaTime * _speedMag;
        }

        // targetに向かって移動
        _enemy.agent.SetDestination(_nextPos);
        _enemy.agent.nextPosition = _enemy.transform.position;

        // 到着
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
