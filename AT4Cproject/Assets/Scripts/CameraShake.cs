using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    [SerializeField]
    private float _minShakeInterval;

    private float _maxShakeInterval = 1f;

    private float _shakeInterval;

    private bool _isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        _shakeInterval = _maxShakeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Mathf.Abs(Input.GetAxis("Horizontal"));
        float vertical = Mathf.Abs(Input.GetAxis("Vertical"));

        float force = horizontal + vertical;
        force = Mathf.Min(1f, force);
        _maxShakeInterval = 1.0f - (force);
        if (_maxShakeInterval < _minShakeInterval) _maxShakeInterval = _minShakeInterval;

        if (force > 0)
        {
            _shakeInterval += Time.deltaTime;
            _isMove = true;
        }
        else
        {
            _shakeInterval = 0f;
            if(_isMove)
            {
                _impulseSource.GenerateImpulse(0.5f);
                _isMove = false;
            }
        }

        if (_shakeInterval > _maxShakeInterval)
        {
            _shakeInterval = 0f;
            _impulseSource.GenerateImpulse(force);
        }
    }
}
