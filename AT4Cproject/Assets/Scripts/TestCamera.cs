using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float _offsetHeight;
    public float _offsetForward;

    [SerializeField]
    private GameObject _target;

    public float _mouseSpeed = 1.0f;
    public float _controllerSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        //_target = GameObject.FindGameObjectWithTag("Player");
        Cursor.lockState = CursorLockMode.Locked;
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
        Debug.Log(controllerAngle);

        transform.eulerAngles += new Vector3(-angle.y, angle.x);
        transform.eulerAngles += new Vector3(controllerAngle.y, controllerAngle.x);
    }
}
