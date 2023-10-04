using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;
    public GameData gameData;
    private GameDataMgr()
    {
        Debug.Log(Application.persistentDataPath);
        //读取游戏数据
        gameData = JsonMgr.Instance.LoadData<GameData>("GameData");
    }
    public void SaveGameData()
    {
        JsonMgr.Instance.SaveData(gameData, "GameData");
    }
}
