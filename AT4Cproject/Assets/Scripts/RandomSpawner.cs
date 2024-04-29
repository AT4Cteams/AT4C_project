/**
 * @brief 仮で作った、範囲内にSoundを一定時間ごとに生成するクラス
 * @author 村上
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    private float _height = 0f;

    private Vector3 _size = Vector3.zero;

    //[SerializeField]
    //private GameObject _gameObject;

    [SerializeField]
    private float _coolTime = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _size = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (Time.frameCount % _coolTime == 0)
        {
            // プレハブの位置をランダムで設定
            float x = Random.Range(-_size.x, _size.x);
            //float y = Random.Range(-_size.y, _size.y);
            float z = Random.Range(-_size.z, _size.z);

            Vector3 pos = transform.position + new Vector3(x * 3, _height, z * 3);

            int level = Random.Range(0, 5);

            Sound.Generate(level, pos);
        }
    }
}
