using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpectrum : MonoBehaviour
{
    public AudioSpectrum spectrum;
    //オブジェクトの配列（
    public Transform[] cubes;
    //スペクトラムの高さ倍率
    public float scale;

    private void Update()
    {
        int i = 0;
        //transform.LookAt(Camera.main.transform);

        foreach (var cube in cubes)
        {
            //オブジェクトのスケールを取得
            var localScale = cube.localScale;
            //スペクトラムのレベル＊スケールをYスケールに置き換える
            localScale.y = spectrum.Levels[i] * scale;
            cube.localScale = localScale;
            i++;
        }
    }
}