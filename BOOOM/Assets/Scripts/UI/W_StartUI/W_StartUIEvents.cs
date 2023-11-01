using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W_StartUIEvents : MonoBehaviour
{
    public Text bestRecord;
    private int gameTime;
    private int minute;
    private float second;
    public void Start()
    {
        Time.timeScale = 1;
        gameTime = PlayerPrefs.GetInt("gameTime",0);
        minute = gameTime / 60;
        second = gameTime % 60;

        bestRecord.text = "Record" + "\n" + minute + "'" + second + "''";
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
