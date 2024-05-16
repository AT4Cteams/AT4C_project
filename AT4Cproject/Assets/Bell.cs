using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bell : SoundItem
{
    //[SerializeField]
    //private LineRenderer _himo;
    //[SerializeField]
    //private Transform _startPoint;
    //[SerializeField]
    //private Transform _endPoint;

    [SerializeField]
    [Range(0f, 1f)]
    private float _moveSoundThreshold;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //var position = new Vector3[] { _startPoint.position, _endPoint.position };
        //_himo.SetPositions(position);

        // ���C���J�����̎q�I�u�W�F�N�g�ł�������A�ړ����ɗ��炷
        if(IsChildOfMainCamera())
        {
            MoveSound();
        }
    }

    private void MoveSound()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float soundVolume = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        if (Mathf.Abs(horizontal) > _moveSoundThreshold || Mathf.Abs(vertical) > _moveSoundThreshold)
            Sound.AutoAdjustGenerate(soundVolume, 2f, transform.position, _soundVolume);
    }

    protected override void OnColEnter(Collision collision)
    {
        Sound.Generate(_soundVolume, transform.position);
    }

    bool IsChildOfMainCamera()
    {
        // ���C���J�������擾
        Camera mainCamera = Camera.main;

        // ���C���J������Transform
        Transform mainCameraTransform = mainCamera.transform;

        // �e��H���ă��C���J�������e�ɂ��邩�m�F����
        Transform currentParent = this.transform.parent;
        while (currentParent != null)
        {
            if (currentParent == mainCameraTransform)
            {
                return true;
            }
            currentParent = currentParent.parent;
        }

        return false;
    }
}
