using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // �����Ńt�F�[�h�A�E�g
        // �I�������V�[���J��
        SceneManager.LoadScene(sceneName);
        // �����Ńt�F�[�h�C��
    }
}
