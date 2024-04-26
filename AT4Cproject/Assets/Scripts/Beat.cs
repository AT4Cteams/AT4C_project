using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public Shooter _shoooter;

    public float _speed;

    public float _interval;
    public float _offset;

    private float _justTiming;
    private float _time = 0f;
    private float _offsetJust = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        _justTiming = _interval / 2;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _interval)
        {
            _time = 0f;
            _shoooter.Reload();
            FindEnemy();
        }

    }

    public bool CheckBeatTiming()
    {
        if(_time > _justTiming - _offset && _time < _justTiming + _offset)
        {
            return true;
        }

        return false;
    }

    public bool CheckJustBeatTiming()
    {
        if (_time > _justTiming - _offsetJust && _time < _justTiming + _offsetJust)
        {
            return true;
        }

        return false;
    }

    public float GetInterval()
    {
        return _interval;
    }

    public float GetTime()
    {
        return _time;
    }

    private void FindEnemy()
    {
        GameObject target = GameObject.FindWithTag("Enemy");

        float dis = Vector3.Distance(target.transform.position, this.transform.position);

        _interval = dis * _speed;
        if (_interval > 1.5f) _interval = 1.5f;
        else if (_interval < 0.4f) _interval = 0.4f;

        _justTiming = _interval / 2;

    }
}
