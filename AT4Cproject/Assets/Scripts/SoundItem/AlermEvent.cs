using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlermEvent : MonoBehaviour
{
    [SerializeField]
    private AlermClock _alermClock;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        _alermClock.StartAlerm();

        Destroy(this.gameObject);
    }
}
