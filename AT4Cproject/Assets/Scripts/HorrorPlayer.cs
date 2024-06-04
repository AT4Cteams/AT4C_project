using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorrorPlayer : MonoBehaviour
{
    [Header("移動速度")]
    public float _speed = 10f;

    [Header("ジャンプ力")]
    public float _jumpForce = 1f;

    [Header("ダッシュスピード")]
    public float _jetForce = 1f;

    [Header("オブジェクトの発射速度")]
    public float _shootPower = 3000;



    [Header("ライト")]
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

    [Header("オブジェクトを掴める距離")]
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
        if (!_isGrabNob)//ドアノブを掴んでいない時
        {
            Moving();//移動の処理

            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))//spaceかAボタンが押されたら
            {
                Jump();//ジャンプする関数
            }

            if (Input.GetKey(KeyCode.LeftShift))//左Shiftが押されたら
            {
                Jet(); //ダッシュする関数
            }

        }



        //輪郭をつける処理(Outlineコンポーネントがついているかどうかで光らせるオブジェクトを判別している)====================================

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

        //if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//QかL1が押されたら
        //{
        //    if (grabObjL == null)//左手に何も持っていない時
        //    {
        //        if (Physics.Raycast(ray, out hit, _canGrabDistance))//Rayを飛ばしてオブジェクトがあるかチェック
        //        {
        //            if (isCanGrabObject(hit.collider.gameObject))//hitしたオブジェクトが拾えるオブジェクトかチェック
        //            {
        //                grabObjL = hit.collider.gameObject;//当たったオブジェクトをgrabObjLに格納

        //                Grab(grabObjL, leftHand);//掴む関数を呼び出す
        //                Debug.Log("tukami");
        //            }
        //        }
        //    }
        //    else //if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//既に物を持っているとき、もう一度QかL1が押されたら
        //    {
        //        grabObjL.GetComponentInChildren<GimmickBase>().PressedL1();
        //        Debug.Log("release");
        //    }
        //}


        //左手で物を掴む、離す処理======================================================================================
        if (Input.GetKey(KeyCode.Q) || Input.GetKey("joystick button 4"))//QかL1が押されたら
        {
            if (grabObjL == null)//左手に何も持っていない時
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))//Rayを飛ばしてオブジェクトがあるかチェック
                {
                    if (isCanGrabObject(hit.collider.gameObject))//hitしたオブジェクトが拾えるオブジェクトかチェック
                    {
                        grabObjL = hit.collider.gameObject;//当たったオブジェクトをgrabObjLに格納

                        if (isNob(hit.collider.gameObject))//ドアノブだった場合
                        {
                            _isGrabNob = true;//ドアノブ掴んでるboolをtrueに
                            return;
                        }
                        else //ドアノブ以外だった場合
                        {
                            Grab(grabObjL, leftHand);//掴む関数を呼び出す
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))//既に物を持っているとき、もう一度QかL1が押されたら
            {
                //grabObjL.GetComponentInChildren<GimmickBase>().PressedL1();
                Release(grabObjL, _grabObjLScale);//物を手放す関数を呼び出す
                grabObjL = null;//掴んでいるオブジェクトをnullに
            }
            else //何かを左手に持っているとき
            {
                if (isNob(grabObjL))//ドアノブを持っている場合
                {
                    //ドアを動かす処理
                    if (grabObjL.CompareTag("doornob"))
                        grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjL.CompareTag("doornob2"))
                        grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                    // ドアの開閉音
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjL.transform.position, _doorOpenSoundVolume, _visibleSoundWave);
                }
            }
        }
        else if (isNob(grabObjL))//QもL1も押されておらず、ドアノブを持っていた場合
        {
            grabObjL = null;//grabObjをnullに
            _isGrabNob = false;//ドアノブを持っていない判定に
        }
        //===============================================================================================================================


        //右手で物を掴む、離す処理=======================================================================================================
        if (Input.GetKey(KeyCode.E) || Input.GetKey("joystick button 5"))//QかL1が押されたら
        {
            if (grabObjR == null)//左手に何も持っていない時
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))//Rayを飛ばしてオブジェクトがあるかチェック
                {
                    if (isCanGrabObject(hit.collider.gameObject))//hitしたオブジェクトが拾えるオブジェクトかチェック
                    {
                        grabObjR = hit.collider.gameObject;//当たったオブジェクトをgrabObjRに格納

                        if (isNob(hit.collider.gameObject))//ドアノブだった場合
                        {
                            _isGrabNob = true;//ドアノブ掴んでるboolをtrueに
                            return;
                        }
                        else //ドアノブ以外だった場合
                        {
                            Grab(grabObjR, rightHand);//掴む関数を呼び出す
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 5"))//既に物を持っているとき、もう一度QかL1が押されたら
            {
                Release(grabObjR, _grabObjRScale);//物を手放す関数を呼び出す
                grabObjR = null;//掴んでいるオブジェクトをnullに
            }
            else //何かを左手に持っているとき
            {
                if (isNob(grabObjR))//ドアノブを持っている場合
                {
                    //ドアを動かす処理
                    if (grabObjR.CompareTag("doornob"))
                        grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjR.CompareTag("doornob2"))
                        grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                    // ドアの開閉音
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjR.transform.position, _doorOpenSoundVolume, _visibleSoundWave);
                }
            }
        }
        else if (isNob(grabObjR))//QもL1も押されておらず、ドアノブを持っていた場合
        {
            grabObjR = null;//grabObjをnullに
            _isGrabNob = false;//ドアノブを持っていない判定に
        }
        //===============================================================================================================================
        


        //物を投げる処理====================================================

        float tri = Input.GetAxis("LT RT");//L2,R2の値を取得

        if (Input.GetMouseButtonDown(0) || tri < 0)//左クリックかL2が押されたとき
        {
            if (grabObjL == null) return;//何も持っていなかった場合return

            if (!isNob(grabObjL, false) && !grabObjL.CompareTag("candle"))//ドアノブでもキャンドルでもない場合
            {
                //grabObjL.GetComponentInChildren<GimmickBase>().PressedL2();
                Throw(grabObjL);//投げる関数を呼び出す
                grabObjL = null; //持っているオブジェクトをnullに
            }
        }
        else if (Input.GetMouseButtonDown(1) || tri > 0)//右クリックかR2が押されたとき
        {
            if (grabObjR == null) return;//何も持っていなかった場合return

            if (!isNob(grabObjR, false) && !grabObjR.CompareTag("candle"))//ドアノブでもキャンドルでもない場合
            {
                //grabObjR.GetComponentInChildren<GimmickBase>().PressedR2();
                Throw(grabObjR);//投げる関数を呼び出す
                grabObjR = null;//持っているオブジェクトをnullに
            }
        }
        //===================================================================
    }

    //掴めるオブジェクトかどうかを判別するbool
    private bool isCanGrabObject(GameObject gameObject)
    {
        return gameObject.TryGetComponent<Outline>(out Outline component) && gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb);
    }

    //ドアノブかどうかを判定するbool
    private bool isNob(GameObject gameObject, bool nullCheck = true)
    {
        if (nullCheck) return gameObject != null && (gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2"));
        else return gameObject.CompareTag("doornob") || gameObject.CompareTag("doornob2");
    }

    //物を掴む関数
    private void Grab(GameObject gameObject, Transform hand)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.position = hand.position;
        gameObject.transform.SetParent(hand);
        gameObject.transform.localRotation = Quaternion.identity;
    }

    //物を手放す関数
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

    //物を投げる関数
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

        // 足音
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
