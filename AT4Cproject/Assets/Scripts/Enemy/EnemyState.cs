/**
 * @brief エネミーのステート
 * @author 村上
 * 
 * @details enumのEnemyStateで状態を追加し、さらにInitでstateTableに追記すること。
 */

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Playables;

public enum EnemyState
{
    Idle,
    Tracking,
    Wander,
    Attack,
    Captured,
}

public class EnemyStateContext
{
    public IEnemyState _currentState;    // 現在の状態
    IEnemyState _previousState;   // 直前の状態

    // 状態のテーブル
    Dictionary<EnemyState, IEnemyState> _stateTable;

    public void Init(Enemy enemy, EnemyState initState)
    {
        if (_stateTable != null) return; // 何度も初期化しない

        // 各状態選クラスの初期化
        Dictionary<EnemyState, IEnemyState> table = new()
        {
            { EnemyState.Idle, new EnemyStateIdle(enemy) },
            { EnemyState.Tracking, new EnemyStateTracking(enemy) },
            { EnemyState.Wander, new EnemyStateWander(enemy) },
            { EnemyState.Attack, new EnemyStateAttack(enemy) },
            { EnemyState.Captured, new EnemyStateCaptured(enemy) },
        };
        _stateTable = table;
        _currentState = _stateTable[initState];
    }

    // 別の状態に変更する
    public void ChangeState(EnemyState next)
    {
        if (_stateTable == null) return; // 未初期化の時は無視
        //if (_currentState == null || _currentState._state == next)
        //{
        //    return; // 同じ状態には遷移しない
        //}
        // 退場 → 現在状態変更 → 入場
        var nextState = _stateTable[next];
        _previousState = _currentState;
        _previousState?.Exit();
        _currentState = nextState;
        _currentState.Entry();
    }

    // 現在の状態をUpdateする
    public void Update()
    {
        _currentState?.Update();
    }

    public bool CheckState(EnemyState state)
    {
        var State = _stateTable[state];

        return (_currentState == State);
    }
}

public abstract class IEnemyState
{
    public EnemyState _state { get; }

    // 最初に状態に入ったときに実行されるコード
    public abstract void Entry();

    // フレーム単位のロジックで、新しい状態に移行するための条件を含む
    public abstract void Update();

    // 状態を抜けるときに実行されるコード
    public abstract void Exit();
}