using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
    private Camera _camera;

    public float _offsetHeight;
    public float _offsetForward;

    [SerializeField]
    private GameObject _target;

    public float _mouseSpeed = 1.0f;
    public float _controllerSpeed = 2.0f;

    private float _time = 0f;

    [SerializeField]
    private Image _blackBack;

    private bool _enabled = false;



    private Vector3 _rawPosition = Vector3.zero;

    private HorrorPlayer _player;

    private bool _isMove = false;

    private float _stepCycle;
    private float _nextStep;
    private float _stepInterval = 5f;

    private float _addHeight = 0f;

    [SerializeField]
    [Header("歩く時の揺れの強さ")]
    [Range(0.01f, 0.15f)]
    private float _maxDownHeight;
    [SerializeField]
    [Header("移動をやめたときの戻るときの速さ")]
    [Range(0.001f, 0.01f)]
    private float _returnHeightSpeed;
    private bool _returnHeightMode = false;
    private float _prevStepCycle = 0f;

    [SerializeField]
    [Header("捕まった時の振り返る速度")]
    private float _gameOverRotSpeed;

    [SerializeField]
    [Header("捕まった時のブラックアウトの速度")]
    private float _gameOverFadeSpeed;

    private CinemachineImpulseSource _impulseSource;

    // Start is called before the first frame update
    void Start()
    {
        _camera = this.GetComponent<Camera>();

        //_target = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;

        transform.localEulerAngles = new Vector3(30.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        StartCoroutine(GameStartMove());

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<HorrorPlayer>();
        //stepInterval = _player.stepInterval * 2f;
        //stepCycle = _player.stepCycle;
        //nextStep = _player.nextStep;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCameraShake();

        _rawPosition = _target.transform.position + (_target.transform.forward * _offsetForward)
                                                + (_target.transform.up * _offsetHeight);

        Vector3 nextPos = _rawPosition;
        nextPos.y += _addHeight;

        transform.position = nextPos;

        rotateCameraAngle();

        Vector3 rot = transform.localEulerAngles;
        rot.z = 0f;
        this.transform.localEulerAngles = rot;
    }
    private void rotateCameraAngle()
    {
        if (!_enabled) return;

        Vector3 angle = new Vector3(
            Input.GetAxis("Mouse X") * _mouseSpeed,
            Input.GetAxis("Mouse Y") * _mouseSpeed,
            0
        );

        Vector3 controllerAngle = new Vector3(
            Input.GetAxis("Horizontal2") * _controllerSpeed,
            Input.GetAxis("Vertical2") * _controllerSpeed,
            0
        );
        
        transform.eulerAngles += new Vector3(-angle.y, angle.x);
        transform.eulerAngles += new Vector3(controllerAngle.y, controllerAngle.x);
    }

    // 移動時の画面揺れ
    private void MoveCameraShake()
    {
        if (!_enabled) return;

        _stepInterval = _player.stepInterval;
        _nextStep = _player.nextStep;
        _stepCycle = _player.stepCycle;

        float velMag = _player.GetComponent<Rigidbody>().velocity.magnitude;

        if(_stepCycle > 0.1f)
        {

            if ((_nextStep - _stepCycle) < _stepInterval / 2)
            {
                _addHeight = _stepInterval - _stepInterval - (_nextStep - _stepCycle);
            }
            else
            {
                _addHeight = -_stepInterval + (_nextStep - _stepCycle);
            }

            velMag = Mathf.Min(10f, velMag);
            float heightPower = Mathf.Max(0.05f, velMag / 10f);

            _addHeight *= _maxDownHeight * heightPower;

        }
        else
        {
            if( _stepCycle <= 0f && _prevStepCycle > 0f)
                StartCoroutine(ReturnToOriginalHeight());
        }

        _prevStepCycle = _stepCycle;
    }

    private IEnumerator ReturnToOriginalHeight()
    {
        if (_returnHeightMode) yield break;
        _returnHeightMode = true;

        while(true)
        {
            if(_stepCycle > 0.1f)
            {
                _returnHeightMode = false;
                yield break;
            }

            if(_addHeight > -0.1f)
            {
                _addHeight -= _returnHeightSpeed;
                yield return null;
            }
            else
            {
                break;
            }
        }

        while(true)
        {
            if (_stepCycle > 0.1f)
            {
                _returnHeightMode = false;
                yield break;
            }

            if (_addHeight < 0f)
            {
                _addHeight += _returnHeightSpeed;
                yield return null;
            }
            else
            {
                _returnHeightMode = false;
                yield break;
            }
        }
    }

    private IEnumerator GameStartMove()
    {
        _enabled = false;

        Vector3 startAngle = transform.localEulerAngles;
        Vector3 targetAngle = new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Color startColor = new Color(0, 0, 0, 1);
        Color targetColor = new Color(0, 0, 0, 0);

        while(_time < 1.0f)
        {
            _time += Time.deltaTime * 1.5f;

            transform.localEulerAngles = Vector3.Slerp(startAngle, targetAngle, _time);

            _blackBack.color = Color.Lerp(startColor, targetColor, _time);

            yield return null;
        }

        _time = 0f;
        _enabled = true;
        yield break;
    }

    private IEnumerator GameOverMove(Vector3 position)
    {

        Quaternion startAngle = transform.rotation;
        Quaternion targetAngle = Quaternion.LookRotation((position - this.transform.position));

        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = new Color(0, 0, 0, 1);

        _impulseSource.GenerateImpulse();

        // エネミーに対して振り向く
        while (_time < 1.0f)
        {
            _time += Time.deltaTime * _gameOverRotSpeed;

            transform.rotation = Quaternion.Slerp(startAngle, targetAngle, _time);

            if(_time < 0.5f)
            {
                _blackBack.color = Color.Lerp(startColor, targetColor, _time + 0.4f);
            }
            else
            {
                _blackBack.color = Color.Lerp(startColor, targetColor, 1.0f - (_time + 0.4f));
            }
            
            yield return null;
        }

        _blackBack.color = startColor;

        _time = 0f;

        // 一定時間固定
        while (_time < 1.0f)
        {
            _time += Time.deltaTime * 2f;

            yield return null;
        }

        _time = 0f;

        // 画面ブラックアウト
        float defaultFOV = _camera.fieldOfView;
        float endFOV = defaultFOV * 0.2f;
        while (_time < 1.0f)
        {
            _time += Time.deltaTime * _gameOverFadeSpeed;

            _blackBack.color = Color.Lerp(startColor, targetColor, _time);
            _camera.fieldOfView = Mathf.Lerp(defaultFOV, endFOV, _time);

            Debug.Log(_camera.fieldOfView);

            _impulseSource.GenerateImpulse();
            yield return null;
        }

        _time = 0f;

        _enabled = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }

    public void GameOver(Vector3 position)
    {
        _enabled = false;
        StartCoroutine(GameOverMove(position));
    }

    public bool GetGameOverEnable()
    {
        return _enabled;
    }
}
