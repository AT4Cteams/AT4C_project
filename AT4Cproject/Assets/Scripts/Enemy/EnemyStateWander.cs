/**
 * @brief 徘徊状態
 * @author 村上
 * 
 * @details 付近のランダム位置まで移動 @n
 *          移動が終わったらIdle状態に遷移
 */

using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class EnemyStateWander : IEnemyState
{
    private float _randomRange = 20f;
    private float _angleSpeedMag = 0.5f;
    private float _speedMag = 0.5f;
    private Vector3 _nextPos = Vector3.zero;
    private Vector3 _prevPos = Vector3.zero;

    private int _randomCount = 0;

    Enemy _enemy;

    public EnemyState _state => EnemyState.Idle;
    public EnemyStateWander(Enemy enemy) => _enemy = enemy;
    public override void Entry() 
    {
        _nextPos = NextRandomPos();
        _prevPos = _nextPos;

        _enemy.agent.SetDestination(_nextPos);
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
        Vector3 nextPos = Vector3.zero;

        while (true)
        {
            Vector2 randValue1 = new Vector2(Random.Range(_randomRange / 2, _randomRange), Random.Range(_randomRange / 2, _randomRange));
            Vector2 randValue2 = new Vector2(Random.Range(-_randomRange / 2, -_randomRange), Random.Range(-_randomRange / 2, -_randomRange));

            int n = Random.Range(0, 2);
            int m = Random.Range(0, 2);

            Vector2 randValue;
            randValue.x = (n == 0) ? randValue1.x : randValue2.x;
            randValue.y = (m == 0) ? randValue1.y : randValue2.y;

            nextPos = new Vector3(_enemy.transform.position.x + randValue.x, _enemy.transform.position.y,
                                    _enemy.transform.position.z + randValue.y);

            if (IsInNavMeshBounds(nextPos))
            {
                _randomCount = 0;
                break;
            }
            else
            {
                _randomCount++;
                if(_randomCount > 10)
                {
                    _randomCount = 0;
                    nextPos = _prevPos;
                    return nextPos;
                }
            }
        }
        return nextPos;
    }

    // 指定された位置がNavMeshの範囲内にあるかどうかを判断するメソッド
    private bool IsInNavMeshBounds(Vector3 point)
    {
        NavMeshHit hit;
        // NavMesh上の点をサンプリングして、それが歩行可能かどうかを確認する
        return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
    }
}
