using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMgr : MonoBehaviour
{
    private static TaskMgr instance;
    public static TaskMgr Instance => instance;

    [Header("任务文本内容")]
    public TextAsset[] texts;

    [HideInInspector]
    public int index;

    [Space]
    [Header("任务数量")]
    public int taskNum;
    [Space]
    [Header("每个任务物品对应的标签，对应任务数量")]
    public string[] taskTag;
    //获取场上所有任务物品
    private Dictionary<int, GameObject[]> keyValuePairs = new Dictionary<int, GameObject[]>();
    //每个任务的物品以及对应的号码
    public Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    //当前任务已经获取的物品号码
    [HideInInspector]
    public List<int> taskList = new List<int>();

    private GameData gameData;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //获取所有任务上的物品
        for (int i = 0; i < taskNum; i++)
        {
            keyValuePairs.Add(i, GameObject.FindGameObjectsWithTag(taskTag[i]));
            for (int j = 0; j < keyValuePairs[i].Length; j++)
            {
                print(keyValuePairs[i][j]);
                keyValuePairs[i][j].SetActive(false);
            }               
        }

        //读出数据
        if (PlayerPrefs.GetInt("continue", 0) == 1)
        {
            gameData = GameDataMgr.Instance.gameData;
            if (gameData == null)
                return;

            //读出任务进度
            index = gameData.taskIndex;

            if (index % 2 != 0)//没接任务
            {
                //获取当前任务的物品
                if (keyValuePairs.ContainsKey(index / 2))
                {
                    for (int i = 0; i < keyValuePairs[index / 2].Length; i++)
                        objects.Add(i, keyValuePairs[index / 2][i]);
                }
                //显示当前任务的数据存在的物品
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].SetActive(true);
                    for (int j = 0; j < gameData.taskList.Count; j++)
                    {
                        //不存在数据保留中，说明没有获取到的
                        if (gameData.taskList[j] == i)
                        {
                            taskList.Add(i);
                            objects[i].SetActive(false);
                            continue;
                        }

                    }
                }
            }               
        }
           
    }

    //相应对话结束后，调用任务物品出现
    public void ShowTaskObjs()
    {
        //获取当前任务的物品
        if (keyValuePairs.ContainsKey(index / 2))
        {
            //清空之前的任务的物品
            objects.Clear();
            taskList.Clear();

            for (int i = 0; i < keyValuePairs[index / 2].Length; i++)
                objects.Add(i, keyValuePairs[index / 2][i]);
        }
        //显示当前任务的所有物品
        for (int i = 0; i < objects.Count; i++)
            objects[i].SetActive(true);

        index++;//进行直接任务对话
    }

    //检测
    public void InteractionEvent()
    {
        if((index+1)%2 == 0)
        {
            if (taskList.Count == keyValuePairs[index/2].Length)//任务完成
                index++;
        }

        if (index >= texts.Length)
            return;
        DialogSystem.Instance.GetTextFromFile(texts[index]);
        DialogSystem.Instance.gameObject.SetActive(true);
    }
}
