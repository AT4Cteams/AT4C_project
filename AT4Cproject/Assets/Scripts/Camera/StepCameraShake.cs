using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCameraShake : MonoBehaviour
{
    private TestCamera _camera;

    private HorrorPlayer _player;

    // ���s���̉�ʗh��̍ۂɎg���ϐ�
    private float _stepCycle;
    private float _nextStep;
    private float _stepInterval = 5f;

    [SerializeField]
    [Header("�������̗h��̋���")]
    [Range(0.01f, 0.15f)]
    private float _maxDownHeight;

    [SerializeField]
    [Header("�ړ�����߂��Ƃ��̖߂�Ƃ��̑���")]
    [Range(0.001f, 0.01f)]
    private float _returnHeightSpeed;
    private bool _returnHeightMode = false;
    private float _prevStepCycle = 0f;

    void Start()
    {
        _camera = GetComponent<TestCamera>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<HorrorPlayer>();

    }

    // �ړ����̉�ʗh��
    public void MoveCameraShake()
    {
        if(!_camera.operationEnable)
        {
            _camera.stepAddHeight = 0f;
            return;
        }

        _stepInterval = _player.stepInterval;
        _nextStep = _player.nextStep;
        _stepCycle = _player.stepCycle;

        float velMag = _player.GetComponent<Rigidbody>().velocity.magnitude;

        if (_stepCycle > 0.1f)
        {

            if ((_nextStep - _stepCycle) < _stepInterval / 2)
            {
                _camera.stepAddHeight = _stepInterval - _stepInterval - (_nextStep - _stepCycle);
            }
            else
            {
                _camera.stepAddHeight = -_stepInterval + (_nextStep - _stepCycle);
            }

            velMag = Mathf.Min(10f, velMag);
            float heightPower = Mathf.Max(0.05f, velMag / 10f);

            _camera.stepAddHeight *= _maxDownHeight * heightPower;

        }
        else
        {
            if (_stepCycle <= 0f && _prevStepCycle > 0f)
                StartCoroutine(ReturnToOriginalHeight());
        }

        _prevStepCycle = _stepCycle;
    }

    private IEnumerator ReturnToOriginalHeight()
    {
        if (_returnHeightMode) yield break;
        _returnHeightMode = true;

        while (true)
        {
            if (_stepCycle > 0.1f)
            {
                _returnHeightMode = false;
                yield break;
            }

            if (_camera.stepAddHeight > -0.1f)
            {
                _camera.stepAddHeight -= _returnHeightSpeed * Time.deltaTime;
                yield return null;
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            if (_stepCycle > 0.1f)
            {
                _returnHeightMode = false;
                yield break;
            }

            if (_camera.stepAddHeight < 0f)
            {
                _camera.stepAddHeight += _returnHeightSpeed * Time.deltaTime;
                yield return null;
            }
            else
            {
                _returnHeightMode = false;
                yield break;
            }
        }
    }
}
