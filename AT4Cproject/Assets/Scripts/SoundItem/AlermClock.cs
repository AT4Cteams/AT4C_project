using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlermClock : SoundItem
{
    [Space]
    [Space]
    [SerializeField]
    private float _interval;
    [SerializeField]
    private int _roopTimes;
    [SerializeField]
    private float _capacitySpeed = 1.0f;

    private bool _playMode = false;

    private float _firstNGTime = 0f;

    private void Update()
    {
        if(_firstNGTime < 10f)
            _firstNGTime += Time.deltaTime;
    }

    protected override void OnColEnter(Collision collision)
    {
        if (_firstNGTime < 3f) return;
        if (_playMode) return;
        if (_rigidbody.velocity.magnitude < _capacitySpeed) return;

        StartAlerm();
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

    public void StartAlerm()
    {
        StartCoroutine(Alerm());
    }
}
