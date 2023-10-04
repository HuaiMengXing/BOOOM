using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public bool data = false;
    //12间房间的位置
    public List<Vector3> roomsPos = new List<Vector3>();
    //人物位置
    public Vector3 playerPos;
    //怪物位置
    public Vector3 monsterPos;
    //任务执行进度
    public int taskIndex = 0;
    //当前任务已经获取的物品号码
    public List<int> taskList = new List<int>();
}
