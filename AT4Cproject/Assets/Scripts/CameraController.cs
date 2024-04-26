using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float _offsetHeight;
    public float _offsetBack;

    public GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        //_target = GameObject.FindGameObjectWithTag("Director");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _target.transform.position + (-_target.transform.forward * _offsetBack) 
                                                + (_target.transform.up * _offsetHeight);

        //transform.rotation = Quaternion.LookRotation(_target.transform.forward, Vector3.up);
        transform.rotation = _target.transform.rotation;
        //transform.eulerAngles = _target.eulerAngles;
        //transform.eulerAngles = new Vector3(0, _target.eulerAngles.y, 0);

    }
}
