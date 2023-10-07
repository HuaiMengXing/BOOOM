using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    [Header("12间房间")]
    public GameObject[] rooms_1;
    public GameObject[] rooms_2;
    public GameObject[] rooms_3;
    public GameObject[] rooms_4;
    public int listnum;
    [Space]
    [Header("时间交换")]
    public float changeMinTime;
    public float changeMaxTime;
    [Space]
    [Header("三层高度")]
    public float[] hight;
    [Space]
    [Header("关门换层时间")]
    public float closeTime = 1f;

    private List<int> rooms = new List<int>();
    private int roomListNumber;
    private float timeIndex;
    private float time;   

    private GameData gameData;

    private void Start()
    {
        timeIndex = Random.Range(changeMinTime, changeMaxTime);

        //读出数据
        if (PlayerPrefs.GetInt("continue", 0) == 1)
        {
            gameData = GameDataMgr.Instance.gameData;
            if (gameData == null)
                return;
            //房间位置调整
            for (int i = 0; i < rooms_1.Length; i++)
                rooms_1[i].transform.position = gameData.roomsPos[i];
            for (int i = rooms_1.Length; i < rooms_2.Length + rooms_1.Length; i++)
                rooms_2[i].transform.position = gameData.roomsPos[i];
            for (int i = rooms_2.Length + rooms_1.Length; i < rooms_3.Length + rooms_2.Length + rooms_1.Length; i++)
                rooms_3[i].transform.position = gameData.roomsPos[i];
            for (int i = rooms_3.Length + rooms_2.Length + rooms_1.Length; i < rooms_4.Length + rooms_3.Length + rooms_2.Length + rooms_1.Length; i++)
                rooms_4[i].transform.position = gameData.roomsPos[i];

            //人物怪物位置面向调整
            Player.Instance.transform.position = gameData.playerPos;
            Monster.Instance.transform.position = gameData.monsterPos;
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > timeIndex)
            ChangeRoomIndex();

        if (Input.GetKeyDown(KeyCode.P))
            SaveAllData();
    }

    public void ChangeRoomIndex()
    {
        int index = 0;
        time = 0;
        timeIndex = Random.Range(changeMinTime, changeMaxTime);//重新索引时间
        roomListNumber = Random.Range(1, listnum + 1);//重新索引交换多少列房间
       
        rooms.Clear();
        for (int i = 1; i <= listnum; i++)
            rooms.Add(i);

        for (int i = 0; i < roomListNumber; i++)
        {
            index = Random.Range(0, rooms.Count);
            StartCoroutine(ChangeRoom(rooms[index]));
            rooms.RemoveAt(index);         
        }
        Invoke("StopIE", 4.5f);
    }

    public void StopIE()
    {
        StopAllCoroutines();
    }

    IEnumerator ChangeRoom(int ListIndex)
    {
        int index = 0;
        List<float> changeHight = new List<float>();
        //重新赋值高度
        changeHight.Clear();
        for (int i = 0; i < hight.Length; i++)
            changeHight.Add(hight[i]);

        switch (ListIndex)
        {
            case 1:
                for (int i = 0; i < rooms_1.Length; i++)
                {
                    rooms_1[i].GetComponent<SetFather>().door[0].CloseDoor();//子物体门如没有关，则关闭
                    rooms_1[i].GetComponent<SetFather>().door[1].CloseDoor();//子物体门如没有关，则关闭                                                                //
                }
                yield return new WaitForSeconds(closeTime);//等待门关了
                for (int i = 0; i < rooms_1.Length; i++)
                {
                    if (rooms_1[i].GetComponentInChildren<CameraMove>())//在这个房间里面，则调用振动
                    {
                        rooms_1[i].GetComponentInChildren<CameraMove>().Shake();
                        Player.Instance.changeRooms = true;
                        Player.Instance.GetComponent<CharacterController>().enabled = false;
                    }

                    index = Random.Range(0, changeHight.Count);
                    rooms_1[i].transform.position = new Vector3(rooms_1[i].transform.position.x, changeHight[index], rooms_1[i].transform.position.z);
                    changeHight.RemoveAt(index);

                    if (rooms_1[i].GetComponentInChildren<CameraMove>())//在这个房间里面
                    {
                        Player.Instance.changeRooms = false;
                        if (!Player.Instance.hideing)
                            Player.Instance.GetComponent<CharacterController>().enabled = true;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < rooms_2.Length; i++)
                {
                    rooms_2[i].GetComponent<SetFather>().door[0].CloseDoor();//子物体门如没有关，则关闭
                    rooms_2[i].GetComponent<SetFather>().door[1].CloseDoor();//子物体门如没有关，则关闭                     
                }
                yield return new WaitForSeconds(closeTime);//等待门关了
                for (int i = 0; i < rooms_2.Length; i++)
                {
                    if (rooms_2[i].GetComponentInChildren<CameraMove>())//在这个房间里面，则调用振动
                    {
                        rooms_2[i].GetComponentInChildren<CameraMove>().Shake();
                        Player.Instance.changeRooms = true;
                        Player.Instance.GetComponent<CharacterController>().enabled = false;
                    }

                    index = Random.Range(0, changeHight.Count);
                    rooms_2[i].transform.position = new Vector3(rooms_2[i].transform.position.x, changeHight[index], rooms_2[i].transform.position.z);
                    changeHight.RemoveAt(index);

                    if (rooms_2[i].GetComponentInChildren<CameraMove>())//在这个房间里面
                    {
                        Player.Instance.changeRooms = false;
                        if (!Player.Instance.hideing)
                            Player.Instance.GetComponent<CharacterController>().enabled = true;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < rooms_3.Length; i++)
                {
                    rooms_3[i].GetComponent<SetFather>().door[0].CloseDoor();//子物体门如没有关，则关闭
                    rooms_3[i].GetComponent<SetFather>().door[1].CloseDoor();//子物体门如没有关，则关闭                    
                }
                yield return new WaitForSeconds(closeTime);//等待门关了
                for (int i = 0; i < rooms_3.Length; i++)
                {
                    if (rooms_3[i].GetComponentInChildren<CameraMove>())//在这个房间里面，则调用振动
                    {
                        rooms_3[i].GetComponentInChildren<CameraMove>().Shake();
                        Player.Instance.changeRooms = true;
                        Player.Instance.GetComponent<CharacterController>().enabled = false;
                    }

                    index = Random.Range(0, changeHight.Count);
                    rooms_3[i].transform.position = new Vector3(rooms_3[i].transform.position.x, changeHight[index], rooms_3[i].transform.position.z);
                    changeHight.RemoveAt(index);

                    if (rooms_3[i].GetComponentInChildren<CameraMove>())//在这个房间里面
                    {
                        Player.Instance.changeRooms = false;
                        if (!Player.Instance.hideing)
                            Player.Instance.GetComponent<CharacterController>().enabled = true;
                    }
                }
                break;
            case 4:
                for (int i = 0; i < rooms_4.Length; i++)
                {
                    rooms_4[i].GetComponent<SetFather>().door[0].CloseDoor();//子物体门如没有关，则关闭
                    rooms_4[i].GetComponent<SetFather>().door[1].CloseDoor();//子物体门如没有关，则关闭                  
                }
                yield return new WaitForSeconds(closeTime);//等待门关了
                for (int i = 0; i < rooms_4.Length; i++)
                {
                    if (rooms_4[i].GetComponentInChildren<CameraMove>())//在这个房间里面，则调用振动
                    {
                        rooms_4[i].GetComponentInChildren<CameraMove>().Shake();
                        Player.Instance.changeRooms = true;
                        Player.Instance.GetComponent<CharacterController>().enabled = false;
                    }

                    index = Random.Range(0, changeHight.Count);
                    rooms_4[i].transform.position = new Vector3(rooms_4[i].transform.position.x, changeHight[index], rooms_4[i].transform.position.z);
                    changeHight.RemoveAt(index);

                    if (rooms_4[i].GetComponentInChildren<CameraMove>())//在这个房间里面
                    {
                        Player.Instance.changeRooms = false;
                        if (!Player.Instance.hideing)
                            Player.Instance.GetComponent<CharacterController>().enabled = true;
                    }
                }
                break;
            default:
                break;
        }
    }


    public void SaveAllData()
    {
        GameDataMgr.Instance.gameData.roomsPos.Clear();//清空之前的数据
        //房间位置保存
        for (int i = 0; i < rooms_1.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(rooms_1[i].transform.position);
        for (int i = rooms_1.Length; i < rooms_2.Length + rooms_1.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(rooms_2[i].transform.position);
        for (int i = rooms_2.Length + rooms_1.Length; i < rooms_3.Length + rooms_2.Length + rooms_1.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(rooms_3[i].transform.position);
        for (int i = rooms_3.Length + rooms_2.Length + rooms_1.Length; i < rooms_4.Length + rooms_3.Length + rooms_2.Length + rooms_1.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(rooms_4[i].transform.position);

        //人物怪物位置面向保存
        if (Player.Instance.hideing)
            GameDataMgr.Instance.gameData.playerPos = Player.Instance.hidePlayerPos;
        else
            GameDataMgr.Instance.gameData.playerPos = Player.Instance.transform.position;

        GameDataMgr.Instance.gameData.monsterPos = Monster.Instance.transform.position;

        //任务进度保存 物品进度保存
        GameDataMgr.Instance.gameData.taskIndex = TaskMgr.Instance.index;
        GameDataMgr.Instance.gameData.taskList.Clear();//清空之前的数据
        for (int i = 0; i < TaskMgr.Instance.taskList.Count; i++)
        {
            GameDataMgr.Instance.gameData.taskList.Add(TaskMgr.Instance.taskList[i]);
        }

        GameDataMgr.Instance.SaveGameData();//保存到文件中
        PlayerPrefs.SetInt("continue", 1);//有数据
    }

    //if (changeNumber == 2)
    //{
    //    index = Random.Range(0, rooms.Count);
    //    index1 = rooms[index];//房间索引
    //    Vector3 pos = rooms_4[index1].transform.position;
    //    rooms.RemoveAt(index);

    //    index_1 = Random.Range(0, rooms.Count);
    //    index1_1 = rooms[index_1];
    //    rooms_4[index1].transform.position = rooms_4[index1_1].transform.position;
    //    rooms_4[index1_1].transform.position = pos;
    //}
}
