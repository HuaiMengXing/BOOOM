using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W_StartUIEvents : MonoBehaviour
{
    public Button enterBtn, leaveBtn;
    public Text bestTimeInfo;


    public void Start()
    {
        
    }

    public void OnEnterBtnClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void OnLeaveBtnClicked()
    {
        
    }
   
}
