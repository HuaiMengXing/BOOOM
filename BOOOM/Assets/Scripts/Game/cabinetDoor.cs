using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinetDoor : MonoBehaviour
{
    public InteractionObj[] Obj;
    [HideInInspector]
    public bool OKPull;

    public bool FindPull()
    {
        OKPull = true;
        for(int i = 0; i < Obj.Length; i++)
        {
            if (Obj[i].isPull && OKPull)
                OKPull = false;
        }
        return OKPull;
    }
}
