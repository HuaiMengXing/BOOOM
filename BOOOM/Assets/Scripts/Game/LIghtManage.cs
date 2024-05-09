using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIghtManage : MonoBehaviour
{
    public Light _light;     //获取物体的Light组件
    public float Min = 1.0f;
    public float Max = 9.0f;
    public float distance = 20f;
    bool flag = true;
    float high = 0f;
    float dic = 0f;

    private void Start()
    {
         _light = GetComponent<Light>();
        if (_light == null)
            _light = GetComponentInChildren<Light>();
    }
    void Update()
    {
        high = transform.position.y - Player.Instance.transform.position.y;
        dic = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (!flag && high > Min && high < Max)
        {
            if(dic < distance)
            {
                _light.enabled = true;
                flag = true;
            }   
        }
        else if (flag && (high <= Min || high >= Max || dic > distance))
        {
            _light.enabled = false;
            flag = false;
        }
    }
}
