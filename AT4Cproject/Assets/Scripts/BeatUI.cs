using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatUI : MonoBehaviour
{
    public Slider _slider;

    public Beat _beat;

    private float _maxValue;
    private float _value = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _maxValue = _beat.GetInterval();
    }

    // Update is called once per frame
    void Update()
    {
        _maxValue = _beat.GetInterval();
        _value = _beat.GetTime();

        _slider.value = _value;
        _slider.maxValue = _maxValue;
    }
}
