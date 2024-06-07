using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
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
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!Camera.main.TryGetComponent<TestCamera>(out TestCamera component)) return;
            if (!Camera.main.GetComponent<TestCamera>().operationEnable) return;

            Camera.main.GetComponent<TestCamera>().GameOver(transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Camera.main.TryGetComponent<TestCamera>(out TestCamera component)) return;
            if (!Camera.main.GetComponent<TestCamera>().operationEnable) return;

            Camera.main.GetComponent<TestCamera>().GameOver(transform.position);
        }
    }
}
