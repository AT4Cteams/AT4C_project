using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [Header("移動速度")]
    public float _speed = 10f;

    [Header("ジャンプ力")]
    public float _jumpForce = 1f;

    [Header("ダッシュスピード")]
    public float _jetForce = 1f;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _aim;
    private Quaternion _playerRotation;

    private Transform _director;

    private bool _isJump = true;

    private BulletController _bulletController;

    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _director = GameObject.FindGameObjectWithTag("Director").transform;

        _playerRotation = _transform.rotation;

        _bulletController = GetComponent<BulletController>();
    }

    void FixedUpdate()
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

        if (Input.GetMouseButton(0))
        {
            _bulletController.Shot();
        }
    }

    private void Moving()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up).normalized;

        Vector3 vel = _horizontalRotation * new Vector3(_horizontal, 0f, _vertical).normalized;
        vel.y = _rigidbody.velocity.y;

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        if (_aim.magnitude > 0.5f)
        {
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime);
        }

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
        _rigidbody.AddForce(transform.forward * _jetForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _isJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
    }
}
