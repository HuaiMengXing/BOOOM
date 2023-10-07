using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraMove : MonoBehaviour
{
    public InteractionPanel Interaction;
    public GameObject target;
    public Vector3 offsetPos;
    public float lookBodyHight;
    public float moveSpeed;
    public float rotationSpeed;
    [Header("震动时间和幅度")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.1f;
    [Header("浮动速度和幅度")]
    [Space]
    public float floatSpeed = 1.0f;
    public float floatAmount = 0.1f;
    [Header("手电筒")]
    public GameObject flashLight;
    [Header("角色死亡")]
    public GameObject deathEvent;


    private bool shaking = false;

    [HideInInspector]
    public bool playerDeath = false;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private RaycastHit hitInfo;
    private InteractionObj interactionObj;
    private TaskMgr taskMgr;
    private float offsetPos_Y;
    private Player player;
    private float posY;
    private float time;
    private void Start()
    {
        offsetPos_Y = offsetPos.y;
        player = Player.Instance;
        posY = 0;
    }

    void Update()
    {
        if (target == null || Time.timeScale == 0)
            return;
        if(player.death)
        {
            if (transform.parent != deathEvent.transform)
            {
                transform.SetParent(deathEvent.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }        
            return;
        }


        if (player != null && player.move != Vector3.zero)
            FloatHead();

        //z方向的偏移
        targetPos = target.transform.position + target.transform.forward * offsetPos.z;
        //y方向的偏移
        targetPos += Vector3.up * (offsetPos.y + posY);
        //x方向的偏移
        targetPos += target.transform.right * offsetPos.x;
        //插值运算 到达指定位置
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        //获取看向某个点的四元数
        targetRot = Quaternion.LookRotation(target.transform.position + Vector3.up * posY + Vector3.up * lookBodyHight + Vector3.up * (offsetPos.y - offsetPos_Y) - transform.position);
        //插值运算向目标位置靠拢
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRot, rotationSpeed * Time.deltaTime);

        //交互
        InteractionEvent();

        //手电筒
        if(Input.GetMouseButtonDown(1))
            flashLight.SetActive(!flashLight.activeInHierarchy);
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
            float Dir = 1;

            // 在指定的时间内执行震动效果
            while (elapsed < shakeDuration)
            {
                if (elapsed > shakeDuration / 2.0f)
                    Dir = -1;

                GetComponent<VignetteAndChromaticAberration>().chromaticAberration += Time.deltaTime * Dir * 160;
                transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeAmount;  // 让相机的位置随机变化一定的幅度
                elapsed += Time.deltaTime;  // 算出时间已经过了多少
                yield return null;
            }
            shaking = false;  // 标记为停止震动
            GetComponent<VignetteAndChromaticAberration>().chromaticAberration = 0.2f;
        }
    }

    public void FloatHead()
    {
        float errorFloat = player.moveSpeed / player.walkMoveSpeed;
       
        if (errorFloat > 1.32)
            errorFloat = 1.32f;
        else if (errorFloat < 0.6)
            errorFloat = 0.6f;

        time += Time.deltaTime * floatSpeed * errorFloat;

        posY = Mathf.Sin(time) * (floatAmount * errorFloat);
    }

    public void InteractionEvent()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 5f, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("NO"))))
        {
            if (LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == "Interaction")
            {
                if (!Interaction.isShow)
                {
                    interactionObj = hitInfo.transform.GetComponent<InteractionObj>();
                    Interaction.textUpdate(interactionObj.txt);
                    Interaction.Show();
                    interactionObj.outlineOpen();
                }
                if (Input.GetKeyDown(KeyCode.E) && Interaction.isShow)
                {
                    //交互处理
                    interactionObj.interactionEvent();
                    Interaction.Hide();
                }
            }else if(hitInfo.transform.tag == "Task" && !DialogSystem.Instance.gameObject.activeInHierarchy)
            {
                if (!Interaction.isShow)
                {
                    taskMgr = hitInfo.transform.GetComponent<TaskMgr>();
                    Interaction.textUpdate("任务"+ ((taskMgr.index+2)/2));
                    Interaction.Show();
                }
                if (Input.GetKeyDown(KeyCode.E) && Interaction.isShow)
                {
                    //交互处理
                    taskMgr.InteractionEvent();
                    Interaction.Hide();
                }
            }
            else if (Interaction.isShow)
                Interaction.Hide();
        }
        else if (Interaction.isShow)
        {
            Interaction.Hide();
            interactionObj.outlineClose();
        }
    }
}
