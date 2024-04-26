using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletController _controller;

    public float _destroyTime = 3f;

    private Rigidbody _rigidbody;

    public TrailRenderer _trail;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.AddForce(transform.forward * _controller._speed);

        _trail.startWidth = transform.localScale.x * 1f;

        Destroy(this.gameObject, _destroyTime );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
