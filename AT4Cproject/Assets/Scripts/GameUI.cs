using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text _decibel;
    [SerializeField]
    private SoundMeasure _measure;
    //[SerializeField]
    //private GameObject _circle;
    //private Vector2 _originalCircleSize;

    void Start()
    {
        //_originalCircleSize = _circle.GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        _decibel.text = _measure.soundValue.ToString("f1") + "db";

        {
            //_circle.GetComponent<RectTransform>().sizeDelta = _originalCircleSize * (_measure.soundValue * 0.01f);
        }
    }


}
