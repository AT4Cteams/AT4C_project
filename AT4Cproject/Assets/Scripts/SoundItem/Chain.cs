using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : SoundItem
{

    protected override void OnPlayerEnter(Collision collision)
    {
        Sound.Generate(_soundVolume, transform.position);
    }
}
