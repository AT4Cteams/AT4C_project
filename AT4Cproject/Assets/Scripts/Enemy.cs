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

    [SerializeField]
    [Range(0.0f, 3.0f)] private float[] _speedMagnification = new float[5];

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

        SetSpeedLevel(targetSound.level);
    }

    private void SetSpeedLevel(SoundLevel level)
    {
        _speed = _originalSpeed * _speedMagnification[(int)level];
    }

    private void OnTriggerStay(Collider other)
    {
        // ���̊��m����
        Sound sound = other.GetComponent<Sound>();
        if(sound && !sound.isHit)
        {
            sound.isHit = true;

            float nextTargetVolume = sound.volume;

            //Debug.Log(nextTargetVolume);

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

                _targetVolume = nextTargetVolume;

                // �X�e�[�g�̕ύX�͂����Ƀ^�[�Q�b�g�̂ݕύX
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
        }
        else
        {
            Debug.Log("�U��������Ȃ�����...");
        }
    }
}
