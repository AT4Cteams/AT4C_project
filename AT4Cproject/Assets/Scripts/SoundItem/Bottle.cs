using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float _soundVolume;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rigidbody.velocity.magnitude < 1f) return;

        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            Sound.Generate(_soundVolume, transform.position);

            Destroy(this.gameObject);
        }
    }
}
