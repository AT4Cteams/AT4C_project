using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : SoundItem
{
    protected override void OnPlayerEnter(Collision collision)
    {
        Sound.Generate(_soundVolume, transform.position);
        PlaySound();
    }
}
