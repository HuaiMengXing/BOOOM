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

    private Outline outline = null;
    private bool door;
    private bool doorIsOpen;
    private Quaternion doorQuaternion;
    private float time;
    void Start()
    {
        doorQuaternion = Quaternion.FromToRotation(transform.right, transform.forward);
        outline = gameObject.GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }
    void Update()
    {
        if(door)
        {
            time += Time.deltaTime * doorSpeed;
            if (doorIsOpen)
            {
                transform.rotation = Quaternion.Lerp(doorQuaternion, Quaternion.identity, time);
                if(time >= 1)
                {
                    door = false;
                    time = 0;
                    transform.rotation = Quaternion.identity;
                    doorIsOpen = false;
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
                Destroy(gameObject);
                break;
            case interactionType.hide:
                break;
            default:
                break;
        }
    }
}
