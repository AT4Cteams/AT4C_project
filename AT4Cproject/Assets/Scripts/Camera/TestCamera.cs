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
    private StepCameraShake _stepCameraShake;
    private CameraEvent _cameraEvent;

    [SerializeField]
    private float _offsetHeight;

    [SerializeField]
    private float _offsetForward;

    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private float _mouseSpeed = 1.0f;

    [SerializeField]
    private float _controllerSpeed = 2.0f;

    // ëÄçÏâ¬î\Ç©Ç«Ç§Ç©
    [HideInInspector]
    public bool operationEnable = false;

    private Vector3 _rawPosition = Vector3.zero;

    private HorrorPlayer _player;

    public float stepAddHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _stepCameraShake = GetComponent<StepCameraShake>();
        _cameraEvent = GetComponent<CameraEvent>();

        transform.localEulerAngles = new Vector3(30.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<HorrorPlayer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _stepCameraShake.MoveCameraShake();

        _rawPosition = _target.transform.position + (_target.transform.forward * _offsetForward)
                                                + (_target.transform.up * _offsetHeight);

        Vector3 nextPos = _rawPosition;
        nextPos.y += stepAddHeight;

        transform.position = nextPos;

        rotateCameraAngle();

        Vector3 rot = transform.localEulerAngles;
        rot.z = 0f;
        this.transform.localEulerAngles = rot;
    }

    private void rotateCameraAngle()
    {
        if (!operationEnable) return;

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

    public void GameOver(Vector3 position)
    {
        _cameraEvent.GameOver(position);
    }
}
