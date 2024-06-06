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

    [SerializeField]
    [Header("捕まえた時のカメラとの距離")]
    [Range(0f, 2f)]
    private float _captureCameraLength;

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

        SetSpeedLevel(_targetVolume);
    }

    private void SetSpeedLevel(float volume)
    {
        _speed = _originalSpeed * (1f + (volume * 0.02f));
    }

    private void OnTriggerStay(Collider other)
    {
        // 音の感知処理
        Sound sound = other.GetComponent<Sound>();
        if(sound && !sound.isHit)
        {
            sound.isHit = true;

            float nextTargetVolume = sound.volume;

            Debug.Log(nextTargetVolume);

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


                // ステートの変更はせずにターゲットのみ変更
                _targetVolume = nextTargetVolume;
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

            if (!Camera.main.TryGetComponent<TestCamera>(out TestCamera component)) return;
            if (!Camera.main.GetComponent<TestCamera>().GetGameOverEnable()) return;

            Vector3 pos = Camera.main.transform.position + (Camera.main.transform.forward * 0.5f);

            Instantiate(_attackEffect, pos, Quaternion.Euler(90f, 0f, 0f));

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;


            ChangeState(EnemyState.Captured);
            LookAtCamera();

            Vector3 headPos = new Vector3(0f, 2f, 0f);

            Camera.main.GetComponent<TestCamera>().GameOver(this.transform.position + headPos);
        }
        else
        {
            Debug.Log("攻撃当たらなかった...");
        }
    }

    private void LookAtCamera()
    {

        Vector3 cpos = (Camera.main.transform.position);

        transform.LookAt(cpos);

        GetComponentInChildren<Collider>().isTrigger = true;

        //this.transform.position = new Vector3(transform.position.x, cpos.y, transform.position.z);

        // 近づく
        {
            Vector3 pos = transform.position;

            Vector3 v = (cpos - transform.position).normalized * 1.0f;

            float len = Vector3.Distance(cpos, transform.position);

            while (len > _captureCameraLength)
            {
                pos += v * 0.1f;

                len = Vector3.Distance(cpos, pos);
            }

            transform.position = pos;
        }
    }
}
