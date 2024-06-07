using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    // �{�^���̃e�L�X�g
    [SerializeField] private List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �I����ԂŐF��ύX����
        foreach(Button button in buttons)
        {
            if (EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                button.GetComponentInChildren<Text>(true).color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                button.GetComponentInChildren<Text>(true).color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
        }
    }

    // �e�X�̃{�^���������ꂽ���̏���
    // �V�K�X�^�[�g
    public void NewGame()
    {
        Debug.Log("NewGame");
    }

    // ��������X�^�[�g
    public void Continue()
    {
        Debug.Log("Continue");
    }

    // �ݒ��ʂ�
    public void Settings()
    {
        Debug.Log("Settings");
    }

    // �Q�[���I��
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
