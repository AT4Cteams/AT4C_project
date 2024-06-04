using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorrorPlayer : MonoBehaviour
{
    [Header("�ړ����x")]
    public float _speed = 10f;

    [Header("�W�����v��")]
    public float _jumpForce = 1f;

    [Header("�_�b�V���X�s�[�h")]
    public float _jetForce = 1f;

    [Header("�I�u�W�F�N�g�̔��ˑ��x")]
    public float _shootPower = 3000;



    [Header("���C�g")]
    private GameObject _light;

    private bool _isLightOn = true;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _aim;
    private Quaternion _playerRotation;

    private bool _isJump = true;

    public Transform rightHand;
    public Transform leftHand;

    [HideInInspector] public GameObject grabObjL;
    [HideInInspector] public GameObject grabObjR;
    private GameObject hitObject;

    [Header("�I�u�W�F�N�g��͂߂鋗��")]
    [SerializeField] private float _canGrabDistance = 5.0f;

    private Vector3 _grabObjLScale = Vector3.one;
    private Vector3 _grabObjRScale = Vector3.one;

    private bool _isGrabNob;

    [SerializeField] private bool _visibleSoundWave = false;
    [SerializeField]
    [Range(0f, 100f)]
    private float _footSoundVolume;
    [SerializeField]
    [Range(0f, 100f)]
    private float _doorOpenSoundVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float _footSoundDeadValue;

    [SerializeField]
    private MeshRenderer _bodyModel;

    private float _doorAngleL;
    private float _doorAngleR;


    public static HorrorPlayer player;


    private void Awake()
    {
        player = this;
    }

    private FootstepSE footstepSE;

    public float stepCycle;
    public float nextStep;
    public float stepInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _playerRotation = _transform.rotation;

        _light = GameObject.Find("FlashLight");

        _bodyModel.enabled = false;

        footstepSE = GetComponent<FootstepSE>();
        stepCycle = 0f;
        nextStep = stepCycle / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGrabNob)//�h�A�m�u��͂�ł��Ȃ���
        {
            Moving();//�ړ��̏���

            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))//space��A�{�^���������ꂽ��
            {
                Jump();//�W�����v����֐�
            }

            if (Input.GetKey(KeyCode.LeftShift))//��Shift�������ꂽ��
            {
                Jet(); //�_�b�V������֐�
            }

        }



        //�֊s�����鏈��(Outline�R���|�[�l���g�����Ă��邩�ǂ����Ō��点��I�u�W�F�N�g�𔻕ʂ��Ă���)====================================

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit _hit;
        RaycastHit hit;

        if (Physics.Raycast(ray, out _hit, _canGrabDistance))
        {
            if (isCanGrabObject(_hit.collider.gameObject))
            {
                if (hitObject != null)
                {
                    hitObject.GetComponent<Outline>().OutlineColor = Color.clear;
                    hitObject = null;
                }

                hitObject = _hit.collider.gameObject;
                hitObject.GetComponent<Outline>().OutlineColor = Color.green;
            }
            else
            {
                if (hitObject != null)
                {
                    hitObject.GetComponent<Outline>().OutlineColor = Color.clear;
                    hitObject = null;
                }
            }
        }
        else
        {
            if (hitObject != null) hitObject.GetComponent<Outline>().OutlineColor = Color.clear;
            hitObject = null;
        }
        //============================================================================================================

        //if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//Q��L1�������ꂽ��
        //{
        //    if (grabObjL == null)//����ɉ��������Ă��Ȃ���
        //    {
        //        if (Physics.Raycast(ray, out hit, _canGrabDistance))//Ray���΂��ăI�u�W�F�N�g�����邩�`�F�b�N
        //        {
        //            if (isCanGrabObject(hit.collider.gameObject))//hit�����I�u�W�F�N�g���E����I�u�W�F�N�g���`�F�b�N
        //            {
        //                grabObjL = hit.collider.gameObject;//���������I�u�W�F�N�g��grabObjL�Ɋi�[

        //                Grab(grabObjL, leftHand);//�͂ފ֐����Ăяo��
        //                Debug.Log("tukami");
        //            }
        //        }
        //    }
        //    else //if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//���ɕ��������Ă���Ƃ��A������xQ��L1�������ꂽ��
        //    {
        //        grabObjL.GetComponentInChildren<GimmickBase>().PressedL1();
        //        Debug.Log("release");
        //    }
        //}


        //����ŕ���͂ށA��������======================================================================================
        if (Input.GetKey(KeyCode.Q) || Input.GetKey("joystick button 4"))//Q��L1�������ꂽ��
        {
            if (grabObjL == null)//����ɉ��������Ă��Ȃ���
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))//Ray���΂��ăI�u�W�F�N�g�����邩�`�F�b�N
                {
                    if (isCanGrabObject(hit.collider.gameObject))//hit�����I�u�W�F�N�g���E����I�u�W�F�N�g���`�F�b�N
                    {
                        grabObjL = hit.collider.gameObject;//���������I�u�W�F�N�g��grabObjL�Ɋi�[

                        if (isNob(hit.collider.gameObject))//�h�A�m�u�������ꍇ
                        {
                            _isGrabNob = true;//�h�A�m�u�͂�ł�bool��true��
                            return;
                        }
                        else //�h�A�m�u�ȊO�������ꍇ
                        {
                            Grab(grabObjL, leftHand);//�͂ފ֐����Ăяo��
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//���ɕ��������Ă���Ƃ��A������xQ��L1�������ꂽ��
            {
                //grabObjL.GetComponentInChildren<GimmickBase>().PressedL1();
                Release(grabObjL, _grabObjLScale);//����������֐����Ăяo��
                grabObjL = null;//�͂�ł���I�u�W�F�N�g��null��
            }
            else //����������Ɏ����Ă���Ƃ�
            {
                if (isNob(grabObjL))//�h�A�m�u�������Ă���ꍇ
                {
                    //�h�A�𓮂�������
                    if (grabObjL.CompareTag("doornob"))
                        grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjL.CompareTag("doornob2"))
                        grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                    // �h�A�̊J��
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjL.transform.position, _doorOpenSoundVolume, _visibleSoundWave);
                }
            }
        }
        else if (isNob(grabObjL))//Q��L1��������Ă��炸�A�h�A�m�u�������Ă����ꍇ
        {
            grabObjL = null;//grabObj��null��
            _isGrabNob = false;//�h�A�m�u�������Ă��Ȃ������
        }
        //===============================================================================================================================


        //�E��ŕ���͂ށA��������=======================================================================================================
        if (Input.GetKey(KeyCode.E) || Input.GetKey("joystick button 5"))//Q��L1�������ꂽ��
        {
            if (grabObjR == null)//����ɉ��������Ă��Ȃ���
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))//Ray���΂��ăI�u�W�F�N�g�����邩�`�F�b�N
                {
                    if (isCanGrabObject(hit.collider.gameObject))//hit�����I�u�W�F�N�g���E����I�u�W�F�N�g���`�F�b�N
                    {
                        grabObjR = hit.collider.gameObject;//���������I�u�W�F�N�g��grabObjR�Ɋi�[

                        if (isNob(hit.collider.gameObject))//�h�A�m�u�������ꍇ
                        {
                            _isGrabNob = true;//�h�A�m�u�͂�ł�bool��true��
                            return;
                        }
                        else //�h�A�m�u�ȊO�������ꍇ
                        {
                            Grab(grabObjR, rightHand);//�͂ފ֐����Ăяo��
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 5"))//���ɕ��������Ă���Ƃ��A������xQ��L1�������ꂽ��
            {
                Release(grabObjR, _grabObjRScale);//����������֐����Ăяo��
                grabObjR = null;//�͂�ł���I�u�W�F�N�g��null��
            }
            else //����������Ɏ����Ă���Ƃ�
            {
                if (isNob(grabObjR))//�h�A�m�u�������Ă���ꍇ
                {
                    //�h�A�𓮂�������
                    if (grabObjR.CompareTag("doornob"))
                        grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjR.CompareTag("doornob2"))
                        grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                    // �h�A�̊J��
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjR.transform.position, _doorOpenSoundVolume, _visibleSoundWave);
                }
            }
        }
        else if (isNob(grabObjR))//Q��L1��������Ă��炸�A�h�A�m�u�������Ă����ꍇ
        {
            grabObjR = null;//grabObj��null��
            _isGrabNob = false;//�h�A�m�u�������Ă��Ȃ������
        }
        //===============================================================================================================================
        


        //���𓊂��鏈��====================================================

        float tri = Input.GetAxis("LT RT");//L2,R2�̒l���擾

        if (Input.GetMouseButtonDown(0) || tri < 0)//���N���b�N��L2�������ꂽ�Ƃ�
        {
            if (grabObjL == null) return;//���������Ă��Ȃ������ꍇreturn

            if (!isNob(grabObjL, false) && !grabObjL.CompareTag("candle"))//�h�A�m�u�ł��L�����h���ł��Ȃ��ꍇ
            {
                //grabObjL.GetComponentInChildren<GimmickBase>().PressedL2();
                Throw(grabObjL);//������֐����Ăяo��
                grabObjL = null; //�����Ă���I�u�W�F�N�g��null��
            }
        }
        else if (Input.GetMouseButtonDown(1) || tri > 0)//�E�N���b�N��R2�������ꂽ�Ƃ�
        {
            if (grabObjR == null) return;//���������Ă��Ȃ������ꍇreturn

            if (!isNob(grabObjR, false) && !grabObjR.CompareTag("candle"))//�h�A�m�u�ł��L�����h���ł��Ȃ��ꍇ
            {
                //grabObjR.GetComponentInChildren<GimmickBase>().PressedR2();
                Throw(grabObjR);//������֐����Ăяo��
                grabObjR = null;//�����Ă���I�u�W�F�N�g��null��
            }
        }
        //===================================================================
    }

    //�͂߂�I�u�W�F�N�g���ǂ����𔻕ʂ���bool
    private bool isCanGrabObject(GameObject gameObject)
    {
        return gameObject.TryGetComponent<Outline>(out Outline component) && gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb);
    }

    //�h�A�m�u���ǂ����𔻒肷��bool
    private bool isNob(GameObject gameObject, bool nullCheck = true)
    {
        if (nullCheck) return gameObject != null && (gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2"));
        else return gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2");
    }

    //����͂ފ֐�
    private void Grab(GameObject gameObject, Transform hand)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.position = hand.position;
        gameObject.transform.SetParent(hand);
        gameObject.transform.localRotation = Quaternion.identity;
    }

    //����������֐�
    private void Release(GameObject gameObject, Vector3 originScale)
    {
        if (gameObject != null)
        {
            if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.transform.SetParent(null);
                gameObject = null;
                _isGrabNob = false;
            }
        }
    }

    //���𓊂���֐�
    private void Throw(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.transform.SetParent(null);
                gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
                gameObject = null;
                _isGrabNob = false;
            }
        }
    }

   
    ///////////////////////////////////////////////////////////////////
   
    private void Moving()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up).normalized;

        Vector3 vel = horizontalRotation * new Vector3(horizontal, 0f, vertical);
        vel.y = _rigidbody.velocity.y;

        //_aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        _transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        _rigidbody.velocity = new Vector3(vel.x * _speed, vel.y, vel.z * _speed);


        float velMag = _rigidbody.velocity.magnitude;
        stepInterval = Mathf.Max(1.0f, Mathf.Min(10f, 15f - velMag));

        // ����
        float soundVolume = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        if (Mathf.Abs(horizontal) > _footSoundDeadValue || Mathf.Abs(vertical) > _footSoundDeadValue)
        {
            stepCycle += Time.deltaTime * _speed;

            Sound.AutoAdjustGenerate(soundVolume, 2f, transform.position, _footSoundVolume, _visibleSoundWave);

            if(stepCycle > nextStep)
            {
                nextStep = stepCycle + stepInterval;
                footstepSE.PlayFootstepSE();
            }
        }
        else
        {
            stepCycle = 0f;
            nextStep = stepInterval;
        }
    }

    private void Jump()
    {
        if (!_isJump) return;
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        //_rigidbody.velocity = Vector3.up * _jumpForce;

        _isJump = false;
    }

    private void Jet()
    {
        Vector3 v = _rigidbody.velocity;
        v.y = 0f;
        v = v.normalized;

        _rigidbody.AddForce(v * _jetForce, ForceMode.Impulse);
        //Sound.Generate(SoundLevel.lv3, transform.position);
    }

    //private void UseLight()
    //{
    //    _isLightOn = (_isLightOn) ? false : true;
    //    _light.SetActive(_isLightOn);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ground")
        {
            _isJump = true;
        }
    }
}
