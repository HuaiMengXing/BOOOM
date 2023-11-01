using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class vector3
{
    public vector3(float X,float Y,float Z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    public vector3()
    {

    }
    public double x;
    public double y;
    public double z;
}
public class GameData
{
    public bool data = false;
    //12间房间的位置
    public List<vector3> roomsPos = new List<vector3>();
    //人物位置
    public vector3 playerPos = new vector3();
    //怪物位置
    public vector3 monsterPos = new vector3();
    //任务执行进度
    public int taskIndex = 0;
    //当前任务已经获取的物品号码
    public List<int> taskList = new List<int>();
}
