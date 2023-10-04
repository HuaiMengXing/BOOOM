using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactionType
{
    singleDoor,
    doubleDoor,
    getObj,
    hide,
}
public class InteractionObj : MonoBehaviour
{
    public interactionType type = interactionType.getObj;
    public string txt;
    public float doorSpeed = 10;
    public AudioClip doorSound_Opend = null;
    public AudioClip doorSound_Close = null;

    private Outline outline = null;
    private bool door;
    private bool doorIsOpen;
    private Quaternion doorQuaternion;
    private float time;
    private AudioSource audioS;
    private bool hideing;
    private Vector3 playerPos;
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        doorQuaternion = Quaternion.FromToRotation(transform.right, transform.forward);
        outline = gameObject.GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }
    void Update()
    {
        Door();       
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
                break;
            case interactionType.getObj:
                GetObj();
                break;
            case interactionType.hide:
                Hide();
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
                transform.rotation = Quaternion.Lerp(doorQuaternion, Quaternion.identity, time);
                if (time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = Quaternion.identity;
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
                transform.rotation = Quaternion.Lerp(Quaternion.identity, doorQuaternion, time);
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
        gameObject.SetActive(false);
    }
    public void CloseDoor()
    {
        if(doorIsOpen)
        {
            interactionEvent();
        }
    }
}
