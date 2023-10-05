using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

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
        Pause = 310,
        Success = 420,
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

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        curElem = new CanvasElem();
        nexElem = new CanvasElem();
        canvasStack = new Stack<CanvasElem>();
    }
    /// <summary>
    /// ��ʾ��ͣ����
    /// </summary>
    public void ShowPause()         
    {
        Time.timeScale = 0;
        Process(CanvasType.Pause, pausePrefab);
    }
    /// <summary>
    /// ��ʾ�ɹ�����
    /// </summary>
    public void ShowSuccess()       
    {                               // +++ �����䣺���³ɹ������ record ��ʾ
        Time.timeScale = 0;
        Process(CanvasType.Success, successPrefab);
    }
    /// <summary>
    /// ��ʾʧ�ܽ���
    /// </summary>
    public void ShowDie()           
    {
        Time.timeScale = 0;
        Process(CanvasType.Die, diePrefab);
    }
 
    /// <summary>
    /// ���ص�ǰ����
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
    public void CloseCanvas()
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
    
    private void Process(CanvasType t, GameObject obj)       //��ʾ����
    {
        if ((int)curElem.type > (int)t) return;            //��Ҫ�Ĳ㼶���ͣ�ֱ�ӷ���
        HideCurrent();
        if ((int)nexElem.type / 100 != (int)t / 100)      //��¼����һ�����Ҫ�Ĳ�ͬ����ʱ
        {
            Destroy(nexElem.canvas);
            nexElem.canvas = null;
        }
        if (nexElem.canvas != null)
        {
            if (curElem.canvas != null) canvasStack.Push(new CanvasElem(curElem.canvas, curElem.type));
            curElem.CopyElem(nexElem);
            nexElem.canvas = null;
        }
        if ((int)curElem.type / 100 == (int)t / 100)       //ͬһ�㼶��,�������ڵ�
        {
            if (curElem.type != t) //Ȼ���ж����ڵĽ����Ƿ�����Ҫ�ģ�������Ҫ�ģ���ջ�����ٵ�
            {
                if (canvasStack.Count != 0) canvasStack.Pop();
                Destroy(curElem.canvas);    //�����ڵ����ٵ�
                CreateCanvasElem(t, obj);
            }
        }
        else                                                    //��Ҫ�Ĳ㼶���ߣ��½�Ȼ����ջ
        {
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
}
