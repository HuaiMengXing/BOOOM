using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactionType
{
    singleDoor,
    doubleDoor,
    getObj,
    hide,
    tv,
    pull_out,
}
public enum dir
{
    X,
    Y,
    Z
}
public class InteractionObj : MonoBehaviour
{
    public interactionType type = interactionType.getObj;
    public dir dir = dir.X;
    public bool reversed;
    public string txt;
    [Header("是否随机")]
    public bool randomObj = false;
    [Header("门或者拉柜")]
    public float doorSpeed = 10;
    public AudioClip Sound_Opend = null;
    public AudioClip doorSound_Close = null;
    [Header("双开门关联")]
    public GameObject otherOneDoor;
    public BoxCollider otherDoorCollider;
    [Header("柜门")]
    public bool isCabinetDoor = false;

    [Header("拉柜距离、速度")]
    public float dis;
    public float pullSpeed = 3;
    [Header("拾取物品音频")]
    public AudioSource getObjSound;
    [Header("触发特殊聊天")]
    public TextAsset text;

    private GameObject[] randomPos;
    private Outline outline = null;
    private bool door;
    private bool doorIsOpen;
    private bool pull;
    [HideInInspector]
    public bool isPull;
    private bool isOpenTv;
    private Quaternion doorQuaternion;
    private Quaternion otherDoorQuaternion;//要旋转的位置
    private Quaternion newdoorQ;//原来的位置
    private Quaternion otherNewdoorQ;
    private float time;
    private AudioSource audioS;
    private bool hideing;
    private Vector3 playerPos;
    private Vector3 pullOutPos;
    private Vector3 nowPullOutPos;
    private Vector3 Objdir;
    void Start()
    {
        switch (dir)
        {
            case dir.X:
                Objdir = transform.right;
                break;
            case dir.Y:
                Objdir = transform.up;
                break;
            case dir.Z:
                Objdir = transform.forward;
                break;
        }
        if(reversed)
            Objdir = -Objdir;

        audioS = GetComponent<AudioSource>();
        doorQuaternion = Quaternion.LookRotation(Objdir);
        newdoorQ = transform.rotation;
        pullOutPos = transform.localPosition;
        if (otherOneDoor!=null)
        {
            otherDoorQuaternion = Quaternion.LookRotation(otherOneDoor.transform.right);
            otherNewdoorQ = otherOneDoor.transform.rotation;
        }
            

        outline = gameObject.GetComponent<Outline>();

        if (randomObj)
            randomObjPos();
    }
    void Update()
    {
        if (type == interactionType.doubleDoor)
            DoubleDoor();
        else if (type == interactionType.singleDoor)
            Door();
        PullOut();
    }

    public void randomObjPos()
    {
        randomPos = GameObject.FindGameObjectsWithTag("Random");
        int id = Random.Range(0, randomPos.Length);
        transform.parent.transform.position = randomPos[id].transform.position;
        transform.parent.transform.SetParent(randomPos[id].transform);
    }

    public void outlineOpen()
    {
        if (outline != null)
            outline.enabled = true;
    }
    public void outlineClose()
    {
        if (outline != null)
            outline.enabled = false;
    }

    public void interactionEvent()
    {
        switch (type)
        {
            case interactionType.singleDoor:
                if (time == 0)
                {
                    door = true;
                    if (doorIsOpen)
                    {
                        if (audioS != null && doorSound_Close != null)
                        {
                            audioS.clip = doorSound_Close;
                            audioS.Play();
                        }
                        txt = "打开";
                    }                     
                    else
                    {
                        if (audioS != null && Sound_Opend != null)
                        {
                            audioS.clip = Sound_Opend;
                            audioS.Play();
                        }
                        txt = "关闭";
                    }
                       
                }                  
                break;
            case interactionType.doubleDoor:
                if(time == 0)
                {
                    door = true;
                    if (audioS != null && Sound_Opend != null)
                    {
                        audioS.clip = Sound_Opend;
                        audioS.Play();
                    }
                    if (doorIsOpen)
                        txt = "开门";
                    else
                        txt = "关门";
                }              
                break;
            case interactionType.getObj:
                GetObj();
                break;
            case interactionType.hide:
                Hide();
                break;
            case interactionType.tv:
                OpendTV();
                break;
            case interactionType.pull_out:
                if (time == 0)
                {
                    if(isCabinetDoor)
                    {
                        if(transform.GetComponent<cabinetDoor>())
                        {
                            if (!transform.GetComponent<cabinetDoor>().FindPull())
                            {
                                txt = "不可";
                                return;
                            }
                            txt = "交互";
                        }
                    }
                    if (audioS != null && Sound_Opend != null)
                    {
                        audioS.clip = Sound_Opend;
                        audioS.Play();
                    }
                    nowPullOutPos = transform.localPosition;
                    pull = true;
                }                  
                break;
            default:
                break;
        }
    }

    public void Door()
    {
        if (door)
        {               
            time += Time.deltaTime * doorSpeed;
            if (doorIsOpen)
            {
                transform.rotation = Quaternion.Lerp(doorQuaternion, newdoorQ, time);
                if (time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = newdoorQ;
                    doorIsOpen = false;
                    if (audioS != null && doorSound_Close != null)
                    {
                        audioS.clip = doorSound_Close;
                        audioS.Play();
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(newdoorQ, doorQuaternion, time);
                if (time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = doorQuaternion;
                    doorIsOpen = true;
                }
            }

        }
    }

    public void DoubleDoor()
    {
        if (door)
        {
            time += Time.deltaTime * doorSpeed;
            if (doorIsOpen)
            {
                transform.rotation = Quaternion.Lerp(doorQuaternion, newdoorQ, time);
                otherOneDoor.transform.rotation = Quaternion.Lerp(otherDoorQuaternion, otherNewdoorQ, time);
                if (time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = newdoorQ;
                    otherOneDoor.transform.rotation = otherNewdoorQ;
                    otherDoorCollider.enabled = true;

                    doorIsOpen = false;
                    if (audioS != null && doorSound_Close != null)
                    {
                        audioS.clip = doorSound_Close;
                        audioS.Play();
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(newdoorQ, doorQuaternion, time);
                otherOneDoor.transform.rotation = Quaternion.Lerp(otherNewdoorQ, otherDoorQuaternion, time);
                if (time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = doorQuaternion;
                    otherOneDoor.transform.rotation = otherDoorQuaternion;
                    doorIsOpen = true;
                    otherDoorCollider.enabled = false;
                }
            }

        }
    }
    public void Hide()
    {
        if(!hideing)
        {
            playerPos = Player.Instance.transform.localPosition;
            Player.Instance.hidePlayerPos = Player.Instance.transform.position;
            Player.Instance.GetComponent<CharacterController>().enabled = false;
            Player.Instance.transform.position = transform.position;
            Player.Instance.hideing = true;
            if(Player.Instance._camera.offsetPos.y < 1)
            {
                Player.Instance._camera.offsetPos.y = 1;
                Player.Instance.moveSpeed = Player.Instance.walkMoveSpeed;
            }    
            hideing = true;
            txt = "离开";
        }else
        {
            Player.Instance.transform.localPosition = playerPos;
            Player.Instance.GetComponent<CharacterController>().enabled = true;
            Player.Instance.hideing = false;
            hideing = false;
            txt = "躲藏";
        }
        
        
    }

    public void OpendTV()
    {
        if(!isOpenTv)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isOpenTv = true;
            txt = "关闭";
        }else
        {
            isOpenTv = false;
            transform.GetChild(0).gameObject.SetActive(false);
            txt = "打开";
        }
        
        
    }

    public void PullOut()
    {
        if (pull)
        {
            time += Time.deltaTime * pullSpeed;
            if (isPull)
            {
                transform.localPosition = Vector3.Lerp(nowPullOutPos, pullOutPos, time);
                if (time >= 1)
                {
                    pull = false;
                    transform.localPosition = pullOutPos;
                    time = 0;
                    isPull = false;
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(pullOutPos, pullOutPos + Objdir * dis, time);
                if (time >= 1)
                {
                    pull = false;
                    transform.localPosition = pullOutPos + Objdir * dis;
                    time = 0;
                    isPull = true;
                }
            }
        }
    }
    public void GetObj()
    {
        if (getObjSound != null)
            getObjSound.Play();

        //属于当前任务的物品
        if (TaskMgr.Instance.objects.ContainsValue(this.gameObject))
        {
            for (int i = 0; i < TaskMgr.Instance.objects.Count; i++)
            {
                if (TaskMgr.Instance.objects[i] == gameObject)
                    TaskMgr.Instance.taskList.Add(i);//记录获取的物品key
            }            
        }
        if(text != null)
        {
            if (TaskMgr.Instance.noTaskObj[TaskMgr.Instance.index / 2] > 0)
                TaskMgr.Instance.noTaskObj[TaskMgr.Instance.index / 2]--;//非任务物品
            DialogSystem.Instance.GetTextFromFile(text);
            DialogSystem.Instance.gameObject.SetActive(true);
        }
           
        gameObject.SetActive(false);
    }
    public void CloseDoor()
    {
        if(doorIsOpen)
        {
            interactionEvent();
        }
    }
    public void PullIsNO()
    {
        if (isPull)
        {
            interactionEvent();
        }
    }

    public void OpenDoor()
    {
        if (!doorIsOpen)
        {
            interactionEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if(type == interactionType.doubleDoor && other.tag == "Monster")
        {
            OpenDoor();
        }
    }
}
