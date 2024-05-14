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

    // Start is called before the first frame update
    void Start()
    {
        //_target = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;

        transform.localEulerAngles = new Vector3(30.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        StartCoroutine(GameStartMove());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _target.transform.position + (_target.transform.forward * _offsetForward)
                                                + (_target.transform.up * _offsetHeight);

        //transform.rotation = Quaternion.LookRotation(_target.transform.forward, Vector3.up);
        //transform.rotation = _target.transform.rotation;
        //transform.eulerAngles = _target.eulerAngles;
        //transform.eulerAngles = new Vector3(0, _target.eulerAngles.y, 0);

        rotateCameraAngle();
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
        Vector3 targetAngle = new Vector3(70f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = new Color(0, 0, 0, 1);

        while (_time < 1.0f)
        {
            _time += Time.deltaTime * 1.5f;

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
