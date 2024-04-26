using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float _interval;

    public float _chikachikaTime;

    public float _blinkingDis;

    public float _maxAngle = 90f;
    public float _minAngle = 10f;


    private float _interval2;
    private float _interval3;
    private float _interval4;
    private float _count = 0f;

    private float _maxintencity;
    private Light _light;


    // Start is called before the first frame update
    void Start()
    {
        _interval2 = _interval + (_chikachikaTime * 2f);
        _interval3 = _interval2 + (_chikachikaTime * 1f);
        _interval4 = _interval3 + (_chikachikaTime * 1.5f);

        _light = this.GetComponent<Light>();
        _maxintencity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpotAngle();


        GameObject target = GameObject.FindWithTag("Enemy");
        if (!target) return;

        float dis = Vector3.Distance(target.transform.position, this.transform.position);

        if (dis < _blinkingDis) Blinking();
        else
        {
            _count = 0f;
            _light.intensity = _maxintencity;
        }
    }

    private void Blinking()
    {
        _count += Time.deltaTime;

        if(_count > _interval)
        {
            _light.intensity = 0f;

            if (_count > _interval2)
            {
                _light.intensity = _maxintencity;

                if (_count > _interval3)
                {
                    _light.intensity = 0f;

                    if (_count > _interval4)
                    {
                        _count = 0f;
                        _light.intensity = _maxintencity;
                    }
                }
            }
        }
    }

    private void UpdateSpotAngle()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            _light.spotAngle += 10.0f;

            if (_light.spotAngle > _maxAngle) _light.spotAngle = _maxAngle;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            _light.spotAngle -= 10.0f;

            if (_light.spotAngle < _minAngle) _light.spotAngle = _minAngle;
        }
    }
}
