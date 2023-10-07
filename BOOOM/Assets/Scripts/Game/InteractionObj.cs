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
public class InteractionObj : MonoBehaviour
{
    public interactionType type = interactionType.getObj;
    public string txt;
    [Header("是否随机")]
    public bool randomObj = false;
    [Header("门或者拉柜")]
    public float doorSpeed = 10;
    public AudioClip doorSound_Opend = null;
    public AudioClip doorSound_Close = null;
    [Header("双开门关联")]
    public GameObject otherOneDoor;

    [Header("拉柜距离")]
    public float dis;
    public float pullSpeed = 3;
    [Header("触发特殊聊天")]
    public TextAsset text;

    private GameObject[] randomPos;
    private Outline outline = null;
    private bool door;
    private bool doorIsOpen;
    private bool pull;
    private bool isPull;
    private bool isOpenTv;
    private Quaternion doorQuaternion;
    private Quaternion otherDoorQuaternion;//要旋转的位置
    private Quaternion newdoorQ;//原来的位置
    private Quaternion otherNewdoorQ;
    private float time;
    private AudioSource audioS;
    private bool hideing;
    private Vector3 playerPos;
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        doorQuaternion = Quaternion.LookRotation(-transform.right);
        newdoorQ = transform.rotation;
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
        this.transform.position = randomPos[id].transform.position;
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
                door = true;
                if (doorIsOpen)
                    txt = "开门";
                else
                    txt = "关门";
                break;
            case interactionType.doubleDoor:
                door = true;
                if (doorIsOpen)
                    txt = "开门";
                else
                    txt = "关门";
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
                pull = true;
                if (isPull)
                    txt = "打开";
                else
                    txt = "推回";
                break;
            default:
                break;
        }
    }

    public void Door()
    {
        if (door)
        {
            if (audioS != null && !audioS.isPlaying)
            {
                audioS.clip = doorSound_Opend;
                audioS.Play();
            }
                
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
                    if (audioS != null)
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
            if (audioS != null && !audioS.isPlaying)
            {
                audioS.clip = doorSound_Opend;
                audioS.Play();
            }

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

                    doorIsOpen = false;
                    if (audioS != null)
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
            if (audioS != null && !audioS.isPlaying)
            {
                audioS.clip = doorSound_Opend;
                audioS.Play();
            }
            time += Time.deltaTime * pullSpeed;
            if (isPull)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * dis, time);
                if (time >= 1)
                {
                    pull = false;
                    transform.position = transform.position - transform.forward * dis;
                    time = 0;
                    isPull = false;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * dis, time);
                if (time >= 1)
                {
                    pull = false;
                    transform.position = transform.position + transform.forward * dis;
                    time = 0;
                    isPull = true;
                }
            }
        }
    }
    public void GetObj()
    {     
        //属于当前任务的物品
        if(TaskMgr.Instance.objects.ContainsValue(this.gameObject))
        {
            for (int i = 0; i < TaskMgr.Instance.objects.Count; i++)
            {
                if (TaskMgr.Instance.objects[i] == gameObject)
                    TaskMgr.Instance.taskList.Add(i);//记录获取的物品key
            }            
        }
        if(text != null)
        {
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

    public void OpenDoor()
    {
        if (!doorIsOpen)
        {
            interactionEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(randomObj && other.tag == "Room" && other.transform.parent == null)
            transform.SetParent(other.transform, false);
    }
}
