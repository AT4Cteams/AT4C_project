using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{
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
    [Range(0.01f, 0.1f)]
    private float _maxDownHeight;

    // Start is called before the first frame update
    void Start()
    {
        //_target = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;

        transform.localEulerAngles = new Vector3(30.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        StartCoroutine(GameStartMove());

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<HorrorPlayer>();
        //stepInterval = _player.stepInterval * 2f;
        //stepCycle = _player.stepCycle;
        //nextStep = _player.nextStep;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // ↓　後で消す
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float v = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            // float speed = _player.speed;
            float speed = 10f;

            if(v > 0)
            {
                _stepCycle += Time.deltaTime * speed;

                if (_stepCycle > _nextStep)
                {
                   _nextStep = _stepCycle + _stepInterval;
                }
            }
            else
            {
                _stepCycle = 0f;
                _nextStep = _stepInterval;
            }

            if ((_nextStep - _stepCycle) < _stepInterval / 2)
            {
                _addHeight = _stepInterval - _stepInterval - (_nextStep - _stepCycle);
            }
            else
            {
                _addHeight = -_stepInterval + (_nextStep - _stepCycle);
            }
        }
        // ↑　後で消す

        //float interval = _player.stepInterval;
        //float step = _player.nextStep - _player.stepCycle;

        //if (step < interval / 2)
        //{
        //    _addHeight = interval - interval - step;
        //}
        //else
        //{
        //    _addHeight = interval + step;
        //}

        _addHeight *= _maxDownHeight;

        if(_stepCycle <= 0.1f)
        {
            ReturnToOriginaHeight();
        }

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

         

        //if (!_enabled) return;

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

    private void ReturnToOriginaHeight()
    {
        if (_addHeight == 0f) return;

        _addHeight += 0.01f * Time.deltaTime;
        if(_addHeight < 0f) _addHeight = 0f;
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

    private IEnumerator GameOverMove()
    {
        Vector3 startAngle = transform.localEulerAngles;
        Vector3 targetAngle = new Vector3(90f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = new Color(0, 0, 0, 1);

        while (_time < 1.0f)
        {
            _time += Time.deltaTime * 1.2f;

            transform.localEulerAngles = Vector3.Slerp(startAngle, targetAngle, _time);

            _blackBack.color = Color.Lerp(startColor, targetColor, _time);

            yield return null;
        }

        _time = 0f;
        _enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }

    public void GameOver()
    {
        _enabled = false;
        StartCoroutine(GameOverMove());
    }

    public bool GetGameOverEnable()
    {
        return _enabled;
    }
}
