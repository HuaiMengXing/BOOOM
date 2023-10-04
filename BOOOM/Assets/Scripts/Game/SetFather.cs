using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFather : MonoBehaviour
{
    public InteractionObj door;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.transform.parent == null)
        {
            print(other + "enter");
            other.transform.SetParent(transform,true);
            Camera.main.transform.SetParent(transform, true);
        }
        if(other.tag == "Monster" && other.transform.parent == null)
            other.transform.SetParent(transform, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.transform.parent != null)
        {
            print(other + "exit");
            other.transform.parent = null;
            Camera.main.transform.parent = null;
        }
        if (other.tag == "Monster" && other.transform.parent != null)
            other.transform.parent = null;
    }

}
