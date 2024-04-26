using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHorror : MonoBehaviour
{
    private GameObject _target;
    private Rigidbody _rigidbody;

    public float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Tracking();
    }

    private void Tracking()
    {
        Vector3 velocity = (_target.transform.position - this.transform.position).normalized;
        _rigidbody.AddForce(velocity * _speed);

        transform.LookAt(_target.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
