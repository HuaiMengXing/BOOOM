using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W_PauseUIEvents : MonoBehaviour
{
    public void OnContinueBtnClicked()
    {
        W_GameUIMgr.Instance.HideCurrent();
        Time.timeScale = 1.0f;
    }
    public void OnMenuBtnClicked()
    {
        W_GameUIMgr.Instance.HideCurrent();
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1.0f;
    }
    public void OnRestartBtnClicked()
    {
        PlayerPrefs.DeleteAll();
        W_GameUIMgr.Instance.HideCurrent();
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1.0f;
    }
}
