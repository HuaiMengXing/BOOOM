using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W_StartUIEvents : MonoBehaviour
{
    public Button startBtn, exitBtn, continueBtn;
    public Text bestTimeInfo;


    public void Start()
    {

    }

    public void OnStartBtnClicked()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(1);
    }
    public void OnContinueBtnClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void OnExitBtnClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
