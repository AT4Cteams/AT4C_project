using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorCamera : MonoBehaviour
{
    public float _offsetHeight;
    public float _offsetForward;

    private GameObject _target;

    public float _speed = 1.0f;

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
            Input.GetAxis("Mouse X") * _speed,
            Input.GetAxis("Mouse Y") * _speed,
            0
        );

        transform.eulerAngles += new Vector3(-angle.y, angle.x);
    }
}
