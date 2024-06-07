using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraEvent : MonoBehaviour
{
    private TestCamera _camera;

    // FOV��ς���p
    private CinemachineVirtualCamera _virtualCamera;

    // ��ʉ��o�̎��Ԍv���ϐ�
    private float _time = 0f;

    [SerializeField]
    [Header("�߂܂������̐U��Ԃ鑬�x")]
    private float _gameOverRotSpeed;

    [SerializeField]
    [Header("�߂܂������̌��ߍ����ҋ@����")]
    private float _gameOverWaitTime;

    [SerializeField]
    [Header("��ʂ��^���ÂȎ���")]
    private float _gameOverBlackTime;

    private CinemachineImpulseSource _impulseSource;

    [SerializeField]
    private Image _blackBack;


    void Start()
    {
        _camera = GetComponent<TestCamera>();

        _virtualCamera = this.GetComponentInChildren<CinemachineVirtualCamera>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();

        GameStart();
    }

    public void GameStart()
    {
        StartCoroutine(GameStartEvent());
    }

    public void GameOver(Vector3 position)
    {
        StartCoroutine(GameOverEvent(position));
    }

    private IEnumerator GameStartEvent()
    {
        _camera.operationEnable = false;

        Vector3 startAngle = transform.localEulerAngles;
        Vector3 targetAngle = new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Color startColor = new Color(0, 0, 0, 1);
        Color targetColor = new Color(0, 0, 0, 0);

        while (_time < 1.0f)
        {
            _time += Time.deltaTime * 1.5f;

            transform.localEulerAngles = Vector3.Slerp(startAngle, targetAngle, _time);

            _blackBack.color = Color.Lerp(startColor, targetColor, _time);

            yield return null;
        }

        _time = 0f;

        _camera.operationEnable = true;

        yield break;
    }

    private IEnumerator GameOverEvent(Vector3 position)
    {
        _camera.operationEnable = false;

        Quaternion startAngle = transform.rotation;
        Quaternion targetAngle = Quaternion.LookRotation((position - this.transform.position));

        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = new Color(0, 0, 0, 1);

        _impulseSource.GenerateImpulse();

        // �G�l�~�[�ɑ΂��ĐU�����
        while (_time < 1.0f)
        {
            _time += Time.deltaTime * _gameOverRotSpeed;

            transform.rotation = Quaternion.Slerp(startAngle, targetAngle, _time);

            if (_time < 0.5f)
            {
                _blackBack.color = Color.Lerp(startColor, targetColor, _time + 0.4f);
            }
            else
            {
                _blackBack.color = Color.Lerp(startColor, targetColor, 1.0f - (_time + 0.4f));
            }

            yield return null;
        }

        _blackBack.color = startColor;

        _virtualCamera.m_Lens.FieldOfView = 20f;

        _time = 0f;

        // ��莞�ԌŒ�
        while (_time < _gameOverWaitTime)
        {
            _time += Time.deltaTime;

            yield return null;
        }

        _time = 0f;

        _blackBack.color = targetColor;

        while (_time < _gameOverBlackTime)
        {
            _time += Time.deltaTime;

            yield return null;
        }

        _time = 0f;

        _camera.operationEnable = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }

}
