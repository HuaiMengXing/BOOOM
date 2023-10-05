using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������
/// </summary>
public class W_Diary : MonoBehaviour
{
    [Multiline(5)]
    public string content;
    public Sprite background;

    [Header("�ռǻ���")]
    public Canvas targetCanvas;

    [Range(0f,1000f)]
    [Header("��Ч�����ٶ�")]
    public float effectSpeed = 2f;

    private bool isOpen = false;        //true���ռǴ���

    private Text targetText;            //targetCanvas�µ��ı���ͼ�����
    private Image targetImage;
    private CanvasGroup targetCanvasGroup;

    public void Awake()
    {
        targetImage = targetCanvas.GetComponentInChildren<Image>();
        targetText = targetImage.GetComponentInChildren<Text>();
        targetCanvasGroup = targetCanvas.GetComponent<CanvasGroup>();
        targetCanvasGroup.alpha = 0;
    }
    public void Update()
    {
        if (isOpen == true && Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
        else if (isOpen == false && Input.GetKeyDown(KeyCode.E))
        {
            Show();
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
        if (isOpen == false && targetCanvasGroup.alpha == 0)  //��������
        {
            targetCanvas.enabled = true;
            while (rate <= 1)
            {
                targetCanvasGroup.alpha = rate;
                targetImage.rectTransform.localScale = new Vector3(rate, rate, 1);
                rate += effectSpeed * 0.02f;
                yield return new WaitForSeconds(0.02f);
            }
            targetImage.rectTransform.localScale = new Vector3(1, 1, 1);
            targetCanvasGroup.alpha = 1;
            isOpen = true;
        }
        else if (isOpen == true && targetCanvasGroup.alpha == 1)                 //�رն���
        {
            while(rate>=0)
            {
                targetCanvasGroup.alpha = rate;
                targetImage.rectTransform.localScale = new Vector3(rate, rate, 1);
                rate -= effectSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            targetImage.rectTransform.localScale = new Vector3(0, 0, 1);
            targetCanvasGroup.alpha = 0;
            isOpen = false;
            targetCanvas.enabled = false;
        }
    }



    //==== for test ====
    public void OnGUI()
    {
        if (GUILayout.Button("---===�����ռ�===---") == true)
        {
            Show();
        }
    }
}
