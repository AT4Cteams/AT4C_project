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

    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    private GameObject grabObjL;
    private GameObject grabObjR;
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

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _playerRotation = _transform.rotation;

        _light = GameObject.Find("FlashLight");

        //_bodyModel.material.color = new UnityEngine.Color(0f, 0f, 0f, 0f);
        _bodyModel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGrabNob)
        {
            Moving();


            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
            {
                Jump();
            }


            if (Input.GetKey(KeyCode.LeftShift))
            {
                Jet();
            }

        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit _hit;
        RaycastHit hit;

        //輪郭をつける処理(Outlineコンポーネントがついているかどうかで光らせるオブジェクトを判別している)
        if (Physics.Raycast(ray, out _hit, _canGrabDistance))
        {
            if (_hit.collider.gameObject.TryGetComponent<Outline>(out Outline component) && _hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (hitObject != null)
                {
                    hitObject.GetComponent<Outline>().OutlineColor = new Color(0, 255, 255, 0);
                    hitObject = null;
                }
                hitObject = _hit.collider.gameObject;
                hitObject.GetComponent<Outline>().OutlineColor = Color.green;
            }
            else
            {
                if (hitObject != null)
                {
                    hitObject.GetComponent<Outline>().OutlineColor = new Color(0, 255, 255, 0);
                    hitObject = null;
                }
            }
        }
        else
        {
            if (hitObject != null) hitObject.GetComponent<Outline>().OutlineColor = new Color(0, 255, 255, 0);
            hitObject = null;
        }



        if (Input.GetKey(KeyCode.Q) || Input.GetKey("joystick button 4"))
        {
            if (grabObjL == null)
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))
                {
                    if (hit.collider.gameObject.TryGetComponent<Outline>(out Outline component) && hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        if (hit.collider.gameObject.CompareTag("doornob") || hit.collider.gameObject.CompareTag("doornob2"))
                        {
                            grabObjL = hit.collider.gameObject;
                            _isGrabNob = true;
                            return;

                        }
                        grabObjL = hit.collider.gameObject;
                        grabObjL.GetComponent<Rigidbody>().isKinematic = true;
                        _grabObjLScale = grabObjL.transform.localScale;
                        grabObjL.transform.position = leftHand.position;
                        grabObjL.transform.SetParent(leftHand.transform);
                        grabObjL.transform.localRotation = Quaternion.identity;
                    }

                }
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))
            {
                if (grabObjL.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    grabObjL.GetComponent<Rigidbody>().isKinematic = false;
                    grabObjL.transform.SetParent(null);
                    grabObjL.transform.localScale = _grabObjLScale;
                    grabObjL = null;
                    _isGrabNob = false;
                }
            }
            else
            {
                // ドア開閉
                if (grabObjL.CompareTag("doornob") || grabObjL.CompareTag("doornob2"))
                {
                   // var q = Quaternion.Euler(0, -45, 0);

                    if (grabObjL.CompareTag("doornob")) grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjL.CompareTag("doornob2")) grabObjL.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                   
                    // ドアの開閉音
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjL.transform.position, _doorOpenSoundVolume,_visibleSoundWave);
                }
            }
        }
        else if (grabObjL != null && (grabObjL.CompareTag("doornob") || grabObjL.CompareTag("doornob2")))
        {
            grabObjL = null;
            _isGrabNob = false;
        }

        float tri = Input.GetAxis("LT RT");

        if (Input.GetMouseButtonDown(0) || tri < 0)
        {
            if (grabObjL != null && !grabObjL.CompareTag("doornob") && !grabObjL.CompareTag("doornob2") && !grabObjL.CompareTag("candle"))
            {
                if (grabObjL.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    grabObjL.GetComponent<Rigidbody>().isKinematic = false;
                    grabObjL.transform.SetParent(null);
                    grabObjL.transform.localScale = _grabObjLScale;
                    grabObjL.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
                    grabObjL = null;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) || tri > 0)
        {
            if (grabObjR != null && !grabObjR.CompareTag("doornob") && !grabObjR.CompareTag("doornob2") && !grabObjR.CompareTag("candle"))
            {
                if (grabObjR.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    grabObjR.GetComponent<Rigidbody>().isKinematic = false;
                    grabObjR.transform.SetParent(null);
                    grabObjR.transform.localScale = _grabObjRScale;
                    grabObjR.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
                    grabObjR = null;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.E) || Input.GetKey("joystick button 5"))
        {
            if (grabObjR == null)
            {
                if (Physics.Raycast(ray, out hit, _canGrabDistance))
                {
                    if (hit.collider.gameObject.TryGetComponent<Outline>(out Outline component) && hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        if (hit.collider.gameObject.CompareTag("doornob") || hit.collider.gameObject.CompareTag("doornob2"))
                        {
                            grabObjR = hit.collider.gameObject;
                            _isGrabNob = true;
                            return;

                        }
                        grabObjR = hit.collider.gameObject;
                        grabObjR.GetComponent<Rigidbody>().isKinematic = true;
                        _grabObjRScale = grabObjR.transform.localScale;
                        grabObjR.transform.position = rightHand.position;
                        grabObjR.transform.SetParent(rightHand.transform);
                        grabObjR.transform.localRotation = Quaternion.identity;
                    }

                }
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetKeyDown("joystick button 5"))
            {
                if (grabObjR.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    grabObjR.GetComponent<Rigidbody>().isKinematic = false;
                    grabObjR.transform.SetParent(null);
                    grabObjR.transform.localScale = _grabObjRScale;
                    grabObjR = null;
                    _isGrabNob = false;
                }
            }
            else
            {
                // ドア開閉
                if (grabObjR.CompareTag("doornob") || grabObjR.CompareTag("doornob2"))
                {
                    //var q = Quaternion.Euler(0, -45, 0);
                    
                    _doorAngleR = grabObjR.transform.root.gameObject.transform.eulerAngles.y;
                    
                    if (grabObjR.CompareTag("doornob")) grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, Input.GetAxis("Vertical") * 2, 0);
                    else if (grabObjR.CompareTag("doornob2")) grabObjR.transform.root.gameObject.transform.eulerAngles += new Vector3(0, -Input.GetAxis("Vertical") * 2, 0);

                    
                    // ドアの開閉音
                    float soundVolume = Mathf.Abs(Input.GetAxis("Vertical"));
                    if (soundVolume > 0.3f)
                        Sound.AutoAdjustGenerate(soundVolume, 1f, grabObjR.transform.position, _doorOpenSoundVolume,_visibleSoundWave);
                }
            }
        }
        else if (grabObjR != null && (grabObjR.CompareTag("doornob") || grabObjR.CompareTag("doornob2")))
        {
            grabObjR = null;
            _isGrabNob = false;
        }





}



   
    ///////////////////////////////////////////////////////////////////
   



    
    ///////////////////////////////////////////////////////
   




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

        // 足音
        float soundVolume = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        if(Mathf.Abs(horizontal) > _footSoundDeadValue || Mathf.Abs(vertical) > _footSoundDeadValue)
            Sound.AutoAdjustGenerate(soundVolume, 2f, transform.position, _footSoundVolume, _visibleSoundWave);
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
