using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpectrum : MonoBehaviour
{
    public AudioSpectrum spectrum;
    //�I�u�W�F�N�g�̔z��i
    public Transform[] cubes;
    //�X�y�N�g�����̍����{��
    public float scale;

    private void Update()
    {
        int i = 0;
        //transform.LookAt(Camera.main.transform);

        foreach (var cube in cubes)
        {
            //�I�u�W�F�N�g�̃X�P�[�����擾
            var localScale = cube.localScale;
            //�X�y�N�g�����̃��x�����X�P�[����Y�X�P�[���ɒu��������
            localScale.y = spectrum.Levels[i] * scale;
            cube.localScale = localScale;
            i++;
        }
    }
}