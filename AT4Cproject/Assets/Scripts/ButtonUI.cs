using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    // ボタンのテキスト
    [SerializeField] private List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 選択状態で色を変更する
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

    // 各々のボタンが押された時の処理
    // 新規スタート
    public void NewGame()
    {
        Debug.Log("NewGame");
    }

    // 続きからスタート
    public void Continue()
    {
        Debug.Log("Continue");
    }

    // 設定画面へ
    public void Settings()
    {
        Debug.Log("Settings");
    }

    // ゲーム終了
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
