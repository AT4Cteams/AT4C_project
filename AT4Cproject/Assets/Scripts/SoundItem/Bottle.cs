using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : SoundItem
{
    [Header("この速度以上でぶつかると割れます")]
    [SerializeField]
    private float _capacitySpeed = 1.0f;

    protected override void OnColEnter(Collision collision)
    {
        if (_rigidbody.velocity.magnitude < _capacitySpeed) return;

        Sound.Generate(_soundVolume, transform.position);

        Destroy(this.gameObject);

        PlaySound();
    }
}
