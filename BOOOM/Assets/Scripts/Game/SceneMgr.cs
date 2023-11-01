using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour
{
    static private SceneMgr instance;
    static public SceneMgr Instance => instance;
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
    private AudioSource _audio;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        timeIndex = Random.Range(changeMinTime, changeMaxTime);
        _audio = GetComponent<AudioSource>();

        //读出数据
        if (PlayerPrefs.GetInt("continue", 0) == 1)
        {
            gameData = GameDataMgr.Instance.gameData;
            if (gameData == null)
                return;
            //房间位置调整
            for (int i = 0; i < rooms_1.Length; i++)
                rooms_1[i].transform.position = new Vector3((float)gameData.roomsPos[i].x, (float)gameData.roomsPos[i].y, (float)gameData.roomsPos[i].z);
            for (int i = 0; i < rooms_2.Length; i++)
                rooms_2[i].transform.position = new Vector3((float)gameData.roomsPos[i + rooms_1.Length].x, (float)gameData.roomsPos[i + rooms_1.Length].y, (float)gameData.roomsPos[i + rooms_1.Length].z);
            for (int i = 0; i < rooms_3.Length; i++)
                rooms_3[i].transform.position = new Vector3((float)gameData.roomsPos[i + rooms_2.Length + rooms_1.Length].x, (float)gameData.roomsPos[i + rooms_2.Length + rooms_1.Length].y, (float)gameData.roomsPos[i + rooms_2.Length + rooms_1.Length].z);
            for (int i = 0; i < rooms_4.Length; i++)
                rooms_4[i].transform.position = new Vector3((float)gameData.roomsPos[i + rooms_2.Length + rooms_3.Length + rooms_1.Length].x, (float)gameData.roomsPos[i + rooms_2.Length + rooms_3.Length + rooms_1.Length].y, (float)gameData.roomsPos[i + rooms_2.Length + rooms_3.Length + rooms_1.Length].z);

            //人物怪物位置面向调整
            Player.Instance.transform.position = new Vector3((float)gameData.playerPos.x, (float)gameData.playerPos.y,(float)gameData.playerPos.z);
            Monster.Instance.transform.position = new Vector3((float)gameData.monsterPos.x, (float)gameData.monsterPos.y, (float)gameData.monsterPos.z);
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > timeIndex)
            ChangeRoomIndex();
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
                        _audio.Play();
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
                        _audio.Play();
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
                        _audio.Play();
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
                        _audio.Play();
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
        print(Application.persistentDataPath);
        GameDataMgr.Instance.gameData.roomsPos.Clear();//清空之前的数据
        //房间位置保存
        for (int i = 0; i < rooms_1.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add( new vector3(rooms_1[i].transform.position.x, rooms_1[i].transform.position.y, rooms_1[i].transform.position.z));
        for (int i = 0; i < rooms_2.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(new vector3(rooms_2[i].transform.position.x, rooms_2[i].transform.position.y, rooms_2[i].transform.position.z));
        for (int i = 0; i < rooms_3.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(new vector3(rooms_3[i].transform.position.x, rooms_3[i].transform.position.y, rooms_3[i].transform.position.z));
        for (int i = 0; i < rooms_4.Length; i++)
            GameDataMgr.Instance.gameData.roomsPos.Add(new vector3(rooms_4[i].transform.position.x, rooms_4[i].transform.position.y, rooms_4[i].transform.position.z));

        //人物怪物位置面向保存
        if (Player.Instance.hideing)
            GameDataMgr.Instance.gameData.playerPos = new vector3(Player.Instance.hidePlayerPos.x, Player.Instance.hidePlayerPos.y, Player.Instance.hidePlayerPos.z) ;
        else
            GameDataMgr.Instance.gameData.playerPos = new vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, Player.Instance.transform.position.z);

        GameDataMgr.Instance.gameData.monsterPos = new vector3(Monster.Instance.transform.position.x, Monster.Instance.transform.position.y, Monster.Instance.transform.position.z); ;

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
}
