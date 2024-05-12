using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Ball : SoundItem
{
    protected override void OnColEnter(Collision collision)
    {
        float value = this.GetComponent<Rigidbody>().velocity.magnitude;
        float maxValue = 30f;

        Sound.AutoAdjustGenerate(value, maxValue, transform.position, _soundVolume);

        PlaySound();
    }

    protected override void OnWallEnter(Collision collision)
    {
        Bounce(collision);
    }
}
