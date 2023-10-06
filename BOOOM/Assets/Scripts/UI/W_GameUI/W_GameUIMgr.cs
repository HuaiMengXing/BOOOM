using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ�����е�UI������
/// </summary>
public class W_GameUIMgr : MonoBehaviour
{
    private static W_GameUIMgr instance;
    public static W_GameUIMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<W_GameUIMgr>();
            }
            return instance;
        }
    }

    enum CanvasType     //ͨ����λ�����ֲ㼶�����õ�UI�㼶��Ҫ����100
    {
        None = -1,
        Pause = 610,
        Success = 620,
        Die = 621,
    }

    struct CanvasElem
    {
        public GameObject canvas;
        public CanvasType type;
        public CanvasElem(GameObject obj = null, CanvasType type = CanvasType.None)
        {
            this.canvas = obj;
            this.type = type;
        }
        public void CopyElem(CanvasElem elem)
        {
            this.canvas = elem.canvas;
            this.type = elem.type;
        }
    }
    [Description("���������UIԤ����")]
    public GameObject pausePrefab, successPrefab, diePrefab;
    [Description("����UI����Ⱦ���")]
    public Camera cma;

    private Stack<CanvasElem> canvasStack;
    private CanvasElem curElem, nexElem;
    private float preTimeScale = 1f;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        preTimeScale = Time.timeScale;
        curElem = new CanvasElem();
        nexElem = new CanvasElem();
        canvasStack = new Stack<CanvasElem>();
    }
    /// <summary>
    /// ��ʾ��ͣ����
    /// </summary>
    public void ShowPause()
    {
        Process(CanvasType.Pause, pausePrefab);
        Time.timeScale = 0;
    }
    /// <summary>
    /// ��ʾ�ɹ�����
    /// </summary>
    /// <param name="timeRecord">ͨ����ʱ���¼</param>
    public void ShowSuccess(string timeRecord = "")
    {                               
        Process(CanvasType.Success, successPrefab);
        curElem.canvas.transform.Find("Record").GetComponentInChildren<Text>().text = "Record: " + timeRecord;
        Time.timeScale = 0;
    }
    /// <summary>
    /// ��ʾʧ�ܽ���
    /// </summary>
    public void ShowDie()
    {
        Process(CanvasType.Die, diePrefab);
        Time.timeScale = 0;
    }

    /// <summary>
    /// ���ص�ǰ����(����ֻ����������ʾ����û�йرյ�)
    /// </summary>
    public void HideCurrent()
    {
        if (curElem.canvas != null) curElem.canvas.SetActive(false);
    }
    /// <summary>
    /// ��ʾ��ǰ����
    /// </summary>
    public void ShowCurrent()
    {
        if (curElem.canvas != null) curElem.canvas.SetActive(true);
    }

    /// <summary>
    /// �رյ�ǰ����
    /// </summary>
    public void CloseCurrent()
    {
        HideCurrent();
        if (nexElem.canvas != null) Destroy(nexElem.canvas);
        nexElem.CopyElem(curElem);
        if (canvasStack.Count == 0)
        {
            curElem.canvas = null;
            curElem.type = CanvasType.None;
            return;
        }
        CanvasElem temp = canvasStack.Pop();
        curElem.CopyElem(temp);
        ShowCurrent();
    }

    private void Process(CanvasType t, GameObject obj)      //��ջ��һЩ���ݽ��в���
    {
        if ((int)curElem.type/100 > (int)t/100) return;             //��Ҫ�򿪵� UI �Ĳ㼶���ͣ�ֱ�ӷ���
        HideCurrent();
        if ((int)nexElem.type / 100 != (int)t / 100)        //��¼�ϴιرյ� UI ����Ҫ���ڲ�ͬ�㼶ʱ
        {                                                   //�����ϴιرյ�
            Destroy(nexElem.canvas);
            nexElem.canvas = null;
        }
        if (nexElem.canvas != null)                         //�ϴιرյĲ�Ϊ��ʱ������ǰ��(�ǿ���״̬��) UI ����ջ��
        {                                                   //�����ϴιرյĸ�������
            if (curElem.canvas != null) canvasStack.Push(new CanvasElem(curElem.canvas, curElem.type));
            curElem.CopyElem(nexElem);
            nexElem.canvas = null;
        }
        if ((int)curElem.type / 100 == (int)t / 100)        //ͬһ�㼶��,�������ڵ�
        {
            if (curElem.type != t)                  //Ȼ���ж����ڵĽ����Ƿ�����Ҫ�ģ�������Ҫ�ģ���ջ�����ٵ�
            {
                Destroy(curElem.canvas);            //�����ڵ����ٵ�
                CreateCanvasElem(t, obj);
            }
        }
        else                                                //��Ҫ�Ĳ㼶���ߣ��½�Ȼ����ջ
        {                                                   //������Ҫ�� UI �����ڲ㼶��ʱ����ջ
            if (curElem.canvas != null) canvasStack.Push(new CanvasElem(curElem.canvas, curElem.type));
            CreateCanvasElem(t, obj);
        }
        ShowCurrent();  //��ʾ����
    }
    private void CreateCanvasElem(CanvasType t, GameObject obj)
    {
        GameObject canvas = Instantiate(obj, this.transform);
        canvas.GetComponent<Canvas>().worldCamera = cma;
        curElem.canvas = canvas;
        curElem.type = t;
    }

    //==== for test ====
    //public void OnGUI()
    //{
    //    if(GUILayout.Button("Pause"))
    //    {
    //        ShowPause();
    //    }
    //    if(GUILayout.Button("Success"))
    //    {
    //        ShowSuccess();
    //    }
    //    if(GUILayout.Button("Die"))
    //    {
    //        ShowDie();
    //    }
    //    if(GUILayout.Button("Hide"))
    //    {
    //        HideCurrent();
    //    }
    //    if(GUILayout.Button("Show"))
    //    {
    //        ShowCurrent();
    //    }
    //    if(GUILayout.Button("Close"))
    //    {
    //        CloseCurrent();
    //    }
    //}
}
