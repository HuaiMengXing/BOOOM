using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开始场景的相机效果类
/// </summary>
public class W_StartUICameraEffect : MonoBehaviour
{
    [Header("相机的转动度数限制")]
    public float xLimit = 30f, yLimit = 30f;
    public float rotateSpeed = 20f;

    private float xBegin = 0;
    private float yBegin = 0;
    private float xRotation = 0, yRotation = 0;

    public void Awake()
    {
        Vector3 camRotation = this.transform.rotation.eulerAngles;
        xBegin = camRotation.x;
        xRotation = camRotation.x;
        yBegin = camRotation.y;
        yRotation = camRotation.y;
    }

    public void Update()
    {
        float xInput = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float yInput = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        if (xInput != 0 || yInput != 0)
        {
            RotateCamare(xInput, yInput);
        }

    }
    private void RotateCamare(float xInput, float yInput)
    {
        xRotation -= yInput;
        xRotation = Mathf.Clamp(xRotation, xBegin - xLimit, xBegin + xLimit);
        yRotation += xInput;
        yRotation = Mathf.Clamp(yRotation, yBegin - yLimit, yBegin + yLimit);
        this.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
