/**
 * @brief �G�l�~�[�̃X�e�[�g
 * @author ����
 * 
 * @details enum��EnemyState�ŏ�Ԃ�ǉ����A�����Init��stateTable�ɒǋL���邱�ƁB
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
    public IEnemyState _currentState;    // ���݂̏��
    IEnemyState _previousState;   // ���O�̏��

    // ��Ԃ̃e�[�u��
    Dictionary<EnemyState, IEnemyState> _stateTable;

    public void Init(Enemy enemy, EnemyState initState)
    {
        if (_stateTable != null) return; // ���x�����������Ȃ�

        // �e��ԑI�N���X�̏�����
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

    // �ʂ̏�ԂɕύX����
    public void ChangeState(EnemyState next)
    {
        if (_stateTable == null) return; // ���������̎��͖���
        //if (_currentState == null || _currentState._state == next)
        //{
        //    return; // ������Ԃɂ͑J�ڂ��Ȃ�
        //}
        // �ޏ� �� ���ݏ�ԕύX �� ����
        var nextState = _stateTable[next];
        _previousState = _currentState;
        _previousState?.Exit();
        _currentState = nextState;
        _currentState.Entry();
    }

    // ���݂̏�Ԃ�Update����
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

    // �ŏ��ɏ�Ԃɓ������Ƃ��Ɏ��s�����R�[�h
    public abstract void Entry();

    // �t���[���P�ʂ̃��W�b�N�ŁA�V������ԂɈڍs���邽�߂̏������܂�
    public abstract void Update();

    // ��Ԃ𔲂���Ƃ��Ɏ��s�����R�[�h
    public abstract void Exit();
}