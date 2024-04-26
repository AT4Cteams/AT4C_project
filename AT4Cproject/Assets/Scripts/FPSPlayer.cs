using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
    [Header("�ړ����x")]
    public float _speed = 10f;

    [Header("�W�����v��")]
    public float _jumpForce = 1f;

    [Header("�_�b�V���X�s�[�h")]
    public float _jetForce = 1f;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _aim;
    private Quaternion _playerRotation;

    private bool _isJump = true;

    public Shooter _shooter;

    private bool _isShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _playerRotation = _transform.rotation;
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

        if (Input.GetMouseButton(0) && !_isShoot)
        {
            _shooter.Shot();
            _isShoot = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _isShoot = false;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
    }
}