using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTester : MonoBehaviour
{
    public float _speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CreateProc();
    }

    private void Move()
    {
        Vector3 currentPos = transform.position;

        if (Input.GetKey(KeyCode.W)) currentPos.z += _speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) currentPos.z -= _speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) currentPos.x -= _speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) currentPos.x += _speed * Time.deltaTime;

        transform.position = currentPos;
    }

    private void CreateProc()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Sound.Generate(SoundLevel.lv1, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Sound.Generate(SoundLevel.lv2, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Sound.Generate(SoundLevel.lv3, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Sound.Generate(SoundLevel.lv4, transform.position);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Sound.Generate(SoundLevel.lv5, transform.position);
    }
}
