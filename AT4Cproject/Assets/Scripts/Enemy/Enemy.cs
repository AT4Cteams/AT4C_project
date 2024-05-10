/**
 * @brief エネミー
 * @author 村上
 * 
 * @details 音が鳴ったらTracking状態に遷移します @n
 *          (OnTriggerStayで音の感知をし、Trackingに遷移) @n
 *          聞いている中で最大音量の音の発生源位置に移動
 */
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Range(1, 30)] private float _speed;
    private float _originalSpeed;

    [SerializeField]
    [Range(0, 500)] private float _angularSpeed;

    [SerializeField]
    [Range(0.0f, 3.0f)] private float[] _speedMagnification = new float[5];

    [Space]

    [SerializeField]
    private GameObject _attackEffect;

    //! エネミーの状態
    private EnemyStateContext _context;

    private Vector3 _targetPos;

    private NavMeshAgent _agent;

    private float _targetVolume = 0f;

    [SerializeField]
    private AttackArea _attackArea;

    [SerializeField]
    public NavMeshSurface navMeshSurface;

    // プロパティ==================================
    public float speed => _speed;
    public float angularSpeed => _angularSpeed;
    public Vector3 targetPos => _targetPos;
    public float targetVolume { get { return _targetVolume; } set { _targetVolume = value; } }
    public NavMeshAgent agent => _agent;

    // ===========================================


    private void Awake()
    {
        _context = new EnemyStateContext();
        _context.Init(this, EnemyState.Wander);
    }
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _originalSpeed = _speed;
    }

    private void Update()
    {
        // 状態ごとのUpdate
        _context.Update();
    }

    public void ChangeState(EnemyState state)
    {
        _context.ChangeState(state);
    }

    private void ChangeTargetSound(Sound targetSound)
    {
        _targetPos = targetSound.transform.position;

        _agent.SetDestination(_targetPos);

        SetSpeedLevel(targetSound.level);
    }

    private void SetSpeedLevel(SoundLevel level)
    {
        _speed = _originalSpeed * _speedMagnification[(int)level];
    }

    private void OnTriggerStay(Collider other)
    {
        // 音の感知処理
        Sound sound = other.GetComponent<Sound>();
        if(sound && !sound.isHit)
        {
            sound.isHit = true;

            float nextTargetVolume = sound.volume;

            //Debug.Log(nextTargetVolume);

            // 音を未感知の場合、ターゲットとステートを変更
            if (_context.CheckState(EnemyState.Idle) || _context.CheckState(EnemyState.Wander) || _context.CheckState(EnemyState.Attack))
            {
                //Debug.Log("音が聞こえた！！");

                _targetVolume = nextTargetVolume;
                ChangeTargetSound(sound);
                ChangeState(EnemyState.Tracking);
            }
            else if (nextTargetVolume >= _targetVolume)
            {
                //Debug.Log("もっと大きい音が聞こえた！！");

                _targetVolume = nextTargetVolume;

                // ステートの変更はせずにターゲットのみ変更
                ChangeTargetSound(sound);
            }
        }
    }

    public virtual void Attack()
    {
        Instantiate(_attackEffect, _attackArea.transform.position, Quaternion.Euler(90f, 0f, 0f));

        if (_attackArea.isHit)
        {
            Debug.Log("攻撃が当たったよ！");
        }
        else
        {
            Debug.Log("攻撃当たらなかった...");
        }
    }
}
