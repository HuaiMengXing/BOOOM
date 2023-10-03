using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;
    public static DialogSystem Instance => instance;

    [Header("UI界面")]
    public TextMeshProUGUI textHint;

    [Header("聊天内容出现间隔时间")]
    public float textSpeed = 0.1f;

    private int index;
    private bool textFinish = false;

    private List<string> textList = new List<string>();
    Coroutine Co;

    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Co = StartCoroutine(SetTextUI());
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0)) && index != textList.Count)
        {
            if (textFinish)
                Co = StartCoroutine(SetTextUI());
            else
            {
                StopCoroutine(Co);
                textHint.text = textList[index].ToString();
                index++;
                textFinish = true;
            }
        }
        if(index == textList.Count) //聊天结束
        {
            this.gameObject.SetActive(false);           
            return;
        }
    }

    public void GetTextFromFile(TextAsset file)
    {
        //清空聊天内容
        textList.Clear();
        index = 0;

        //分割聊天内容
        string[] lineData = file.text.Split('\n');

        foreach (string line in lineData)
        {
            //加入聊天列表中
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinish = false;
        textHint.text = "";
        for (int i = 0; i < textList[index].Length; i++)
        {
            textHint.text += textList[index][i].ToString();
            yield return new WaitForSeconds(textSpeed);
        }
        textFinish = true;
        index++;
    }
}
