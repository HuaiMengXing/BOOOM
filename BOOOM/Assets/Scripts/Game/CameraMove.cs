using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target;
    public Vector3 offsetPos;
    public float lookBodyHight;
    public float moveSpeed;
    public float rotationSpeed;
    [Header("震动时间和幅度")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.1f;

    private bool shaking = false;
    private Vector3 originalPos;

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
        targetRot = Quaternion.LookRotation(target.transform.position + Vector3.up * lookBodyHight - transform.position);
        //插值运算向目标位置靠拢
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRot, rotationSpeed * Time.deltaTime);

    }

    public void Shake()
    {
        StartCoroutine(ShakeCamera());
    }
    IEnumerator ShakeCamera()
    {
        if (shaking == false)
        {
            shaking = true;  // 标记为正在震动
            float elapsed = 0.0f;

            // 在指定的时间内执行震动效果
            while (elapsed < shakeDuration)
            {
                transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeAmount;  // 让相机的位置随机变化一定的幅度
                elapsed += Time.deltaTime;  // 算出时间已经过了多少
                yield return null;
            }
            shaking = false;  // 标记为停止震动
        }
    }
}
