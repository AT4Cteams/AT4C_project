using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeasure : MonoBehaviour
{
    private float _soundValue = 0f;
    private float _prevSoundValue = 0f;

    private float _time = 0f;

    [SerializeField]
    private float _maxTime = 1.5f;

    public float soundValue { get { return _soundValue; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _prevSoundValue = _soundValue;

        if (_prevSoundValue == _soundValue)
        {
            _time += Time.deltaTime;

            if ( _time > _maxTime )
            {
                _prevSoundValue = 0f;
                _soundValue = 0f;
                _time = 0f;
            }
        }
        else
        {
            _time = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // âπÇÃä¥ímèàóù
        if (other.gameObject.TryGetComponent<Sound>(out Sound sound))
        {
            //if (sound.isHit) return;

            float nextVolume = sound.volume;

            if(_soundValue < nextVolume)
            {
                _soundValue = nextVolume;
            }


            
        }
    }
}
