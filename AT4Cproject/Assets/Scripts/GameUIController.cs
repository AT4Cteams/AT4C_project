using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Image _aim;

    private Vector2 _aimOriginalSize;
    private Vector2 _aimBigSize;
    private bool _isBig = false;

    public Beat _beat;

    // Start is called before the first frame update
    void Start()
    {
        _aimOriginalSize = _aim.GetComponent<RectTransform>().sizeDelta;
        _aimBigSize = _aimOriginalSize * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        _isBig = !_beat.CheckJustBeatTiming();
        ChangeSize();
    }

    private void ChangeSize()
    {
        _aim.rectTransform.sizeDelta = (_isBig) ? _aimOriginalSize : _aimBigSize;
    }
}
