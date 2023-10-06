using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// 游戏场景中的UI管理器
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

    enum CanvasType     //通过百位来区分层级，有用的UI层级都要大于100
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
    [Description("各个界面的UI预制体")]
    public GameObject pausePrefab, successPrefab, diePrefab;
    [Description("各个UI的渲染相机")]
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
    /// 显示暂停界面
    /// </summary>
    public void ShowPause()
    {
        Process(CanvasType.Pause, pausePrefab);
        Time.timeScale = 0;
    }
    /// <summary>
    /// 显示成功界面
    /// </summary>
    /// <param name="timeRecord">通过的时间记录</param>
    public void ShowSuccess(string timeRecord = "")
    {                               
        Process(CanvasType.Success, successPrefab);
        curElem.canvas.transform.Find("Record").GetComponentInChildren<Text>().text = "Record: " + timeRecord;
        Time.timeScale = 0;
    }
    /// <summary>
    /// 显示失败界面
    /// </summary>
    public void ShowDie()
    {
        Process(CanvasType.Die, diePrefab);
        Time.timeScale = 0;
    }

    /// <summary>
    /// 隐藏当前界面(仅仅只是让它不显示，并没有关闭掉)
    /// </summary>
    public void HideCurrent()
    {
        if (curElem.canvas != null) curElem.canvas.SetActive(false);
    }
    /// <summary>
    /// 显示当前界面
    /// </summary>
    public void ShowCurrent()
    {
        if (curElem.canvas != null) curElem.canvas.SetActive(true);
    }

    /// <summary>
    /// 关闭当前界面
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

    private void Process(CanvasType t, GameObject obj)      //对栈和一些数据进行操作
    {
        if ((int)curElem.type/100 > (int)t/100) return;             //想要打开的 UI 的层级更低，直接返回
        HideCurrent();
        if ((int)nexElem.type / 100 != (int)t / 100)        //记录上次关闭的 UI 和想要的在不同层级时
        {                                                   //销毁上次关闭的
            Destroy(nexElem.canvas);
            nexElem.canvas = null;
        }
        if (nexElem.canvas != null)                         //上次关闭的不为空时，将当前的(是开启状态的) UI 加入栈中
        {                                                   //并将上次关闭的给到现在
            if (curElem.canvas != null) canvasStack.Push(new CanvasElem(curElem.canvas, curElem.type));
            curElem.CopyElem(nexElem);
            nexElem.canvas = null;
        }
        if ((int)curElem.type / 100 == (int)t / 100)        //同一层级的,隐藏现在的
        {
            if (curElem.type != t)                  //然后判断现在的界面是否是想要的，不是想要的，出栈并销毁掉
            {
                Destroy(curElem.canvas);            //把现在的销毁掉
                CreateCanvasElem(t, obj);
            }
        }
        else                                                //想要的层级更高，新建然后入栈
        {                                                   //仅在想要的 UI 比现在层级高时才入栈
            if (curElem.canvas != null) canvasStack.Push(new CanvasElem(curElem.canvas, curElem.type));
            CreateCanvasElem(t, obj);
        }
        ShowCurrent();  //显示界面
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
