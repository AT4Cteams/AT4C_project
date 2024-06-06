/**
 * @brief �G�l�~�[
 * @author ����
 * 
 * @details ����������Tracking��ԂɑJ�ڂ��܂� @n
 *          (OnTriggerStay�ŉ��̊��m�����ATracking�ɑJ��) @n
 *          �����Ă��钆�ōő剹�ʂ̉��̔������ʒu�Ɉړ�
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

    //! �G�l�~�[�̏��
    private EnemyStateContext _context;

    private Vector3 _targetPos;

    private NavMeshAgent _agent;

    private float _targetVolume = 0f;

    [SerializeField]
    private AttackArea _attackArea;

    [SerializeField]
    public NavMeshSurface navMeshSurface;

    [SerializeField]
    [Header("�߂܂������̃J�����Ƃ̋���")]
    [Range(0f, 2f)]
    private float _captureCameraLength;

    // �v���p�e�B==================================
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
        // ��Ԃ��Ƃ�Update
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
        // ���̊��m����
        Sound sound = other.GetComponent<Sound>();
        if(sound && !sound.isHit)
        {
            sound.isHit = true;

            float nextTargetVolume = sound.volume;

            Debug.Log(nextTargetVolume);

            // ���𖢊��m�̏ꍇ�A�^�[�Q�b�g�ƃX�e�[�g��ύX
            if (_context.CheckState(EnemyState.Idle) || _context.CheckState(EnemyState.Wander) || _context.CheckState(EnemyState.Attack))
            {
                //Debug.Log("�������������I�I");

                _targetVolume = nextTargetVolume;
                ChangeTargetSound(sound);
                ChangeState(EnemyState.Tracking);
            }
            else if (nextTargetVolume >= _targetVolume)
            {
                //Debug.Log("�����Ƒ傫���������������I�I");


                // �X�e�[�g�̕ύX�͂����Ƀ^�[�Q�b�g�̂ݕύX
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
            Debug.Log("�U��������������I");

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
            Debug.Log("�U��������Ȃ�����...");
        }
    }

    private void LookAtCamera()
    {

        Vector3 cpos = (Camera.main.transform.position);

        transform.LookAt(cpos);

        GetComponentInChildren<Collider>().isTrigger = true;

        //this.transform.position = new Vector3(transform.position.x, cpos.y, transform.position.z);

        // �߂Â�
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
