using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("’e")]
    public GameObject _bullet;

    [Header("”­ŽË‘¬“x")]
    public float _speed;

    private bool _ready = true;

    public float _offsetForward;

    public Beat _beat;

    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position;
        transform.position += _target.transform.forward * _offsetForward;

        transform.rotation = Camera.main.transform.rotation;
    }

    public void Shot()
    {
        if (!_ready) return;

        if (_beat.CheckBeatTiming())
        {
            _Shot(transform.position, transform.rotation, 10.0f);
        }
        else
        {
            _Shot(transform.position, transform.rotation);
        }

        _ready = false;
    }
    private void _Shot(Vector3 pos, Quaternion rot)
    {
        GameObject bullet = Instantiate(_bullet, pos, rot);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _speed);
    }
    private void _Shot(Vector3 pos, Quaternion rot, float size)
    {
        GameObject bullet = Instantiate(_bullet, pos, rot);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.localScale = bullet.transform.localScale * size;
        rb.AddForce(transform.forward * _speed);
    }

    public void Reload()
    {
        _ready = true;
    }
}
