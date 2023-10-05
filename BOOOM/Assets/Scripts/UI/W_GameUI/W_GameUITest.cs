using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class W_GameUITest : MonoBehaviour
{
    public float speed = 0.1f;
    public bool flag = false;

    float x = 1f;

    public void Update()
    {
        this.transform.position += new Vector3(speed * x*Time.deltaTime, speed * x * Time.deltaTime, speed * x * Time.deltaTime);
        if (this.transform.position.x >= 5)
        {
            x = -1;
        }
        if (this.transform.position.x < -5)
        {
            x = 1;
        }
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            W_GameUIMgr.Instance.ShowPause();
        }
    }



    
    public void OnGUI()
    {
        if (GUILayout.Button("Pause"))
        {
            W_GameUIMgr.Instance.ShowPause();
        }
        if (GUILayout.Button("Success"))
        {
            W_GameUIMgr.Instance.ShowSuccess();
        }
        if (GUILayout.Button("Die"))
        {
            W_GameUIMgr.Instance.ShowDie();
        }
        if (GUILayout.Button("Close"))
        {
            W_GameUIMgr.Instance.CloseCanvas();
        }
        if (GUILayout.Button("Hide"))
        {
            W_GameUIMgr.Instance.HideCurrent();
        }
        if(GUILayout.Button("Show"))
        {
            W_GameUIMgr.Instance.ShowCurrent();
        }
    }
}
