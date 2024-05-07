/**
 * @brief �G�l�~�[
 * @author ����
 * 
 * @details ����������Idle��Ԃ���Tracking��ԂɑJ�ڂ��܂� @n
 *          (OnTriggerStay�ŉ��̊��m�����ATracking�ɑJ��) @n
 *          �����Ă��钆�ōő僌�x���̉���ǔ�(�������x���̏ꍇ�A�Ō�ɕ���������ǔ�)
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Range(5, 100)] private float _speed;
    private float _originalSpeed;

    [SerializeField]
    [Range(0, 500)] private float _angularSpeed;

    [SerializeField]
    [Range(0.0f, 3.0f)] private float[] _speedMagnification = new float[5];

    [Space]

    // �ړI�n�ɒ��������ɐ�������"���̃G�t�F�N�g"
    [SerializeField]
    private GameObject _particleSystem;

    //! �G�l�~�[�̏��
    private EnemyStateContext _context;

    // �Ō�ɕ��������̃��x��
    private SoundLevel _listenedSoundLv;

    private Sound _targetSound;

    private Vector3 _targetPos;

    private NavMeshAgent _agent;

    private float _targetVolume = 0f;

    // �v���p�e�B==================================
    public float speed => _speed;
    public float angularSpeed => _angularSpeed;
    public SoundLevel listenedSoundLv
    { get { return _listenedSoundLv; } set { _listenedSoundLv = value; } }
    public Vector3 targetPos => _targetPos;
    public NavMeshAgent agent => _agent;

    // ===========================================


    private void Awake()
    {
        // State��������
        _context = new EnemyStateContext();
        _context.Init(this, EnemyState.Idle);
    }
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _originalSpeed = _speed;
    }

    // Update is called once per frame
    private void Update()
    {
        // ��Ԃ��Ƃ�Update
        _context.Update();
    }

    public void ChangeState(EnemyState state)
    {
        _context.ChangeState(state);
    }

    public void ChangeTargetSound(Sound targetSound)
    {
        _targetSound = targetSound;
        _targetPos = _targetSound.transform.position;
        _listenedSoundLv = targetSound.level;
        _agent.SetDestination(_targetPos);

        SetSpeedLevel(_targetSound.level);
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

            float dist = Vector3.Distance(transform.position, other.transform.position);

            float nextTargetVolume = _TargetVolumeCalc(dist, (float)sound.level);

            // ���𖢊��m�̏ꍇ�A�^�[�Q�b�g�ƃX�e�[�g��ύX
            if (_context.CheckState(EnemyState.Idle))
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

    public void EffectTest()
    {
        Instantiate(_particleSystem, transform.position, Quaternion.Euler(90f, 0f, 0f));
    }

    public float _TargetVolumeCalc(float dist, float level)
    {
        float maxDistance = 50f;

        dist = Mathf.Min(maxDistance - 0.01f, dist);

        float gennsui = (1.0f - (dist / maxDistance)) * 0.7f;

        float nextTargetVolume = (((float)level + 1f) * gennsui);

        Debug.Log(nextTargetVolume);


        return nextTargetVolume;
    }
}
