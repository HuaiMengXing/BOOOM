using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

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
    [Description("各个界面的UI预制体")]
    public GameObject pausePrefab, successPrefab, diePrefab;
    [Description("各个UI的渲染相机")]
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
    /// 显示暂停界面
    /// </summary>
    public void ShowPause()         
    {
        Time.timeScale = 0;
        Process(CanvasType.Pause, pausePrefab);
    }
    /// <summary>
    /// 显示成功界面
    /// </summary>
    public void ShowSuccess()       
    {                               // +++ 待补充：更新成功界面的 record 显示
        Time.timeScale = 0;
        Process(CanvasType.Success, successPrefab);
    }
    /// <summary>
    /// 显示失败界面
    /// </summary>
    public void ShowDie()           
    {
        Time.timeScale = 0;
        Process(CanvasType.Die, diePrefab);
    }
 
    /// <summary>
    /// 隐藏当前界面
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
    
    private void Process(CanvasType t, GameObject obj)       //显示界面
    {
        if ((int)curElem.type > (int)t) return;            //想要的层级更低，直接返回
        HideCurrent();
        if ((int)nexElem.type / 100 != (int)t / 100)      //记录的下一层和想要的不同级别时
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
        if ((int)curElem.type / 100 == (int)t / 100)       //同一层级的,隐藏现在的
        {
            if (curElem.type != t) //然后判断现在的界面是否是想要的，不是想要的，出栈并销毁掉
            {
                if (canvasStack.Count != 0) canvasStack.Pop();
                Destroy(curElem.canvas);    //把现在的销毁掉
                CreateCanvasElem(t, obj);
            }
        }
        else                                                    //想要的层级更高，新建然后入栈
        {
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
}
