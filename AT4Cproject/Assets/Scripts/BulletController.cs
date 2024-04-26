using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("íe")]
    public GameObject _bullet;

    [Header("î≠éÀë¨ìx")]
    public float _speed;

    [Header("î≠éÀÉåÅ[Ég")]
    public float _rate;

    private float _coolTime = 0f;

    private bool _active = true;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _player.transform.position;

        CoolTime();
    }

    public void Shot()
    {
        if (!_active) return;

        GameObject bullet = Instantiate(_bullet, _player.transform.position + (_player.transform.forward * 3.0f), _player.transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _speed);

        _active = false;
        _coolTime = 0f;
    }
    public void Shot(Vector3 pos, Quaternion rot)
    {
        if (!_active) return;

        GameObject bullet = Instantiate(_bullet, pos, rot);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _speed);

        _active = false;
        _coolTime = 0f;
    }
    public void Shot(Vector3 pos, Quaternion rot, float size)
    {
        if (!_active) return;

        GameObject bullet = Instantiate(_bullet, pos, rot);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.localScale = bullet.transform.localScale * size;
        rb.AddForce(transform.forward * _speed);

        _active = false;
        _coolTime = 0f;
    }

    private void CoolTime()
    {
        _coolTime += Time.deltaTime;

        if (_coolTime > _rate)
        {
            _coolTime = _rate;
            _active = true;
        }
    }
}
