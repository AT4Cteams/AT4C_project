using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectController : MonoBehaviour
{

    public float _speed = 1.0f;

    private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position;
        //transform.rotation = Camera.main.transform.rotation;

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
