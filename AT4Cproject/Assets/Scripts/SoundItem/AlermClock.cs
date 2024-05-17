using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlermClock : SoundItem
{
    [SerializeField]
    private float _interval;
    [SerializeField]
    private int _roopTimes;

    private bool _playMode = false;

    private bool _once = true;

    protected override void OnColEnter(Collision collision)
    {
        if (_playMode) return;
        StartCoroutine(Alerm());
    }

    protected override void OnPlayerEnter(Collision collision)
    {
        if (!_once) return;

        StartCoroutine(Alerm());

        _once = false;
    }

    private IEnumerator Alerm()
    {
        PlaySound();

        for (int ii = 0; ii < _roopTimes; ii++)
        {
            for (int i = 0; i < 4; i++)
            {
                Sound.Generate(_soundVolume, transform.position);

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(_interval);
        }

        _playMode = false;
        yield break;
    }
}
