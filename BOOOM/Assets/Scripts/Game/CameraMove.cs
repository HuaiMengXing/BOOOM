using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target;
    public Vector3 offsetPos;
    public float lookBodyHight;
    public float moveSpeed;
    public float offsetRight;
    public float rotationSpeed;

    [HideInInspector]
    public bool playerDeath = false;
    private Vector3 targetPos;
    private Quaternion targetRot;

    void Update()
    {
        if (target == null || Time.timeScale == 0)
            return;

        //z方向的偏移
        targetPos = target.transform.position + target.transform.forward * offsetPos.z;
        //y方向的偏移
        targetPos += Vector3.up * offsetPos.y;
        //x方向的偏移
        targetPos += target.transform.right * offsetPos.x;
        //插值运算 到达指定位置
        transform.position = Vector3.Lerp(transform.position,targetPos, moveSpeed * Time.deltaTime);

        //获取看向某个点的四元数
        targetRot = Quaternion.LookRotation(target.transform.right * offsetRight + target.transform.position + Vector3.up * lookBodyHight - transform.position);
        //插值运算向目标位置靠拢
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRot, rotationSpeed * Time.deltaTime);

    }
}
