using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField]
    [Range(0, 5)]
    private int _soundLevel;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rigidbody.velocity.magnitude < 1f) return;

        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            Sound.Generate((SoundLevel)_soundLevel, transform.position);

            Destroy(this.gameObject, 0.1f);
        }
    }
}
