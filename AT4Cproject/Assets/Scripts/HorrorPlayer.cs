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

    [Header("オブジェクトの発射速度")]
    public GameObject instantiateObject;


    [Header("ライト")]
    private GameObject _light;

    private bool _isLightOn = true;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _aim;
    private Quaternion _playerRotation;

    private bool _isJump = true;

    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    private float rayDistance = 0.2f;
    private GameObject grabObj;
    private GameObject hitObject;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _playerRotation = _transform.rotation;

        _light = GameObject.Find("FlashLight");
    }

    // Update is called once per frame
    void Update()
    {
        Moving();


        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Jet();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            UseLight();
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;

        //輪郭をつける処理(Outlineコンポーネントがついているかどうかで光らせるオブジェクトを判別している)
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hitObject = hit.collider.gameObject;
            if (hitObject.TryGetComponent<Outline>(out Outline component))
            {
                hitObject.GetComponent<Outline>().OutlineColor = new Color(0, 255, 255, 255);
            }

        }
        else
        {
            if (hitObject != null)
            {
                if (hitObject.TryGetComponent<Outline>(out Outline component))
                {
                    hitObject.GetComponent<Outline>().OutlineColor = new Color(0, 255, 255, 0);
                    hitObject = null;
                }
            }
        }


        if (Input.GetKeyUp(KeyCode.F))
        {


            if (grabObj == null)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    grabObj = hit.collider.gameObject;
                    grabObj.GetComponent<Rigidbody>().isKinematic = true;
                    grabObj.transform.position = grabPoint.position;
                    grabObj.transform.SetParent(grabPoint.transform);
                }
            }
            else
            {
                grabObj.GetComponent<Rigidbody>().isKinematic = false;
                grabObj.transform.SetParent(null);
                grabObj.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
                grabObj = null;
            }


        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    GameObject projectile = Instantiate(instantiateObject,transform.position,Quaternion.identity);
        //    projectile.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward.normalized * _shootPower);
        //}
    }

    private void Moving()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up).normalized;

        Vector3 vel = _horizontalRotation * new Vector3(_horizontal, 0f, _vertical);
        vel.y = _rigidbody.velocity.y;

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        _transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        _rigidbody.velocity = new Vector3(vel.x * _speed, vel.y, vel.z * _speed);
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
        Sound.Generate(SoundLevel.lv3, transform.position);
    }

    private void UseLight()
    {
        _isLightOn = (_isLightOn) ? false : true;
        _light.SetActive(_isLightOn);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ground")
        {
            _isJump = true;
        }
    }
}
