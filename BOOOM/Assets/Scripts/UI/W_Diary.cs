using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 日历类
/// </summary>
public class W_Diary : MonoBehaviour
{
    public string content;
    public Sprite background;

    [Header("日记画布")]
    public Canvas targetCanvas;

    [Range(0f,1000f)]
    [Header("特效播放速度")]
    public float effectSpeed = 2f;

    private bool isOpen = false;        //true：日记打开了

    private Text targetText;            //targetCanvas下的文本框、图像组件
    private Image targetImage;
    private CanvasGroup targetCanvasGroup;

    public void Awake()
    {
        targetText = targetCanvas.GetComponentInChildren<Text>();
        targetImage = targetCanvas.GetComponentInChildren<Image>();
        targetCanvasGroup = targetCanvas.GetComponent<CanvasGroup>();
        targetCanvasGroup.alpha = 0;
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(2)) 
        {
            Hide();
        }
    }
    public void Show()
    {
        if (isOpen == true) return;
        targetText.text = content;
        targetImage.sprite = background;
        StartCoroutine(PlayEffect());
    }
    public void Hide() 
    {
        if (isOpen == false) return;
        StartCoroutine(PlayEffect());
    }
    private IEnumerator<WaitForSeconds> PlayEffect()
    {
        float rate = isOpen ? 1 : 0;
        if (isOpen == false && targetCanvasGroup.alpha == 0)  //开启动画
        {
            targetCanvas.enabled = true;
            while (rate < 1)
            {
                targetCanvasGroup.alpha = rate;
                targetText.rectTransform.localScale = new Vector3(rate, rate, 1);
                targetImage.rectTransform.localScale = new Vector3(rate, rate, 1);
                rate += effectSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            targetCanvasGroup.alpha = 1;
            isOpen = true;
        }
        else if (isOpen == true && targetCanvasGroup.alpha == 1)                 //关闭动画
        {
            while(rate>0)
            {
                targetCanvasGroup.alpha = rate;
                targetText.rectTransform.localScale = new Vector3(rate, rate, 1);
                targetImage.rectTransform.localScale = new Vector3(rate, rate, 1);
                rate -= effectSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            targetCanvasGroup.alpha = 0;
            isOpen = false;
            targetCanvas.enabled = false;
        }
    }



    //==== for test ====
    public void OnGUI()
    {
        if (GUILayout.Button("开启日记") == true)
        {
            Show();
        }
    }
}
