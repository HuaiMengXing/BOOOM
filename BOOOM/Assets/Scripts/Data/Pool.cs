using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private static Pool instance;
    public static Pool Instance => instance;

    private Queue<GameObject> objPoolQueue = new Queue<GameObject>();
    private GameObject[] objPools;
    public GameObject objPool;
    public int maxNum;

    private void Awake()
    {
        instance = this;      
    }
    private void Start()
    {
        objPools = gameObject.GetComponentsInChildren<GameObject>();
        for(int i = 0; i < objPools.Length; i++)
            objPoolQueue.Enqueue(objPools[i]);
    }

    public GameObject Get()
    {
        if(objPoolQueue.Count > 0)
            return objPoolQueue.Dequeue();
        else
            return Instantiate(objPool);
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);//失活
        if(objPoolQueue.Count >= maxNum)
            Destroy(obj);
        else
            objPoolQueue.Enqueue(obj);
    }

    public GameObject GetEff()
    {
        if (objPoolQueue.Count > 0)
            return objPoolQueue.Dequeue();
        else
            return Instantiate(objPool);
    }

    public void PushEff(GameObject obj)
    {
        obj.GetComponent<ParticleSystem>().Stop();
        obj.SetActive(false);//失活
        if (objPoolQueue.Count >= maxNum)
            Destroy(obj);
        else
            objPoolQueue.Enqueue(obj);
    }
}
