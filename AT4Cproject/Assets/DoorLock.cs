using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.TryGetComponent<Key>(out Key key))
        //{
        //    Destroy(this.gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Key>(out Key key)) 
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
