using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W_PauseUIEvents : MonoBehaviour
{
    public void OnContinueBtnClicked()
    {
        W_GameUIMgr.Instance.BkMusic.UnPause();
        W_GameUIMgr.Instance.HideCurrent();
        Time.timeScale = 1;
    }
    public void OnMenuBtnClicked()
    {
        W_GameUIMgr.Instance.HideCurrent();
        if (!Player.Instance.death && !W_GameUIMgr.Instance.success)
            SceneMgr.Instance.SaveAllData();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync(0);
    }
    public void OnRestartBtnClicked()
    {
        PlayerPrefs.DeleteAll();
        W_GameUIMgr.Instance.HideCurrent();
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
    }
}
