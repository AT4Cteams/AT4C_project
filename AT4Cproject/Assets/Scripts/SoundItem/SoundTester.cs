using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundTester : MonoBehaviour
{
    public float _speed = 10.0f;

    private Vector3 _aim;
    private Rigidbody _rigidbody;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CreateProc();
    }

    private void Move()
    {
        //Vector3 currentPos = transform.position;

        //if (Input.GetKey(KeyCode.W)) currentPos.z += _speed * Time.deltaTime;
        //if (Input.GetKey(KeyCode.S)) currentPos.z -= _speed * Time.deltaTime;
        //if (Input.GetKey(KeyCode.A)) currentPos.x -= _speed * Time.deltaTime;
        //if (Input.GetKey(KeyCode.D)) currentPos.x += _speed * Time.deltaTime;

        //transform.position = currentPos;

        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up).normalized;

        Vector3 vel = _horizontalRotation * new Vector3(_horizontal, 0f, _vertical);
        vel.y = _rigidbody.velocity.y;

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        _transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        _rigidbody.velocity = new Vector3(vel.x * _speed, vel.y, vel.z * _speed);
    }

    private void CreateProc()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Sound.Generate(SoundLevel.lv1, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Sound.Generate(SoundLevel.lv2, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Sound.Generate(SoundLevel.lv3, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Sound.Generate(SoundLevel.lv4, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Sound.Generate(SoundLevel.lv5, transform.position);
    }
}
