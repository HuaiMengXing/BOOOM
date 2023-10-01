using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanel : MonoBehaviour
{
    private CanvasGroup canvas;
    [HideInInspector]
    public bool isShow;
    public TextMeshProUGUI textHint;
    [Header("透明化速度")]
    public float alphaSpeed = 10;
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if(isShow && canvas.alpha != 1)
        {
            canvas.alpha += alphaSpeed * Time.deltaTime;
            if(canvas.alpha >= 1)
                canvas.alpha = 1;
        }else if(!isShow && canvas.alpha != 0)
        {
            canvas.alpha -= alphaSpeed * Time.deltaTime;
            if( canvas.alpha <= 0)
                canvas.alpha = 0;
        }
    }

    public virtual void Show()
    {        
        canvas.alpha = 0;
        isShow = true;
    }
    public virtual void Hide()
    {
        canvas.alpha = 1;
        isShow = false;
    }

    public void textUpdate(string txt)
    {
        textHint.text = txt;
    }
}
