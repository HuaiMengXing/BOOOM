using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Transform eatPos;
    public float eatSpeed;
    public TextAsset text;
    public float overTime = 1.5f;

    private bool one = true;
    private bool showOverPanel = false;
    private float time = 0;
    void Update()
    {
        if(Player.Instance.death && !showOverPanel)
        {
            time += Time.deltaTime;
            if(time > overTime)
            {
                showOverPanel = true;
                W_GameUIMgr.Instance.ShowDie();
            }
            if(text != null && one)
            {
                one = false;
                DialogSystem.Instance.GetTextFromFile(text);
                DialogSystem.Instance.gameObject.SetActive(true);
            }
            transform.position = Vector3.Lerp(transform.position, eatPos.position, eatSpeed * Time.deltaTime);   
        }
    }
}
