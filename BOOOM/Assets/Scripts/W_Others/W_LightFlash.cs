using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 灯光闪烁脚本
/// </summary>
public class W_LightFlash : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float posibility = 0.4f;
    public float len = 1f;

    public AnimationCurve curve;                                    //控制灯光变化的曲线
    public float lightning = 1.0f;

    public Light _light => this.GetComponentInChildren<Light>();     //获取物体的Light组件

    public float speed = 1f;
    [Range(0f, 1000f)]
    public float minGapTime = 5f;                                   //闪烁的最小间隔时间

    [Range(0f, 1000f)]
    public float maxGapTime = 3f;                                   //闪烁的最大间隔时间

    private float startIntensity = 10f;                             //Light的起使亮度

    private float x = 0f;
    private AudioSource source;

    bool flag = true;
    float high = 0f;
    float dic = 0f;

    private void Start()
    {
        if (minGapTime < len) minGapTime = len;           //对非法数值进行处理(可能需要优化)
        if (maxGapTime < minGapTime) maxGapTime = minGapTime;
        startIntensity = _light.intensity;
        source = GetComponent<AudioSource>();
        StartCoroutine(Flash());
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        high = transform.position.y - Player.Instance.transform.position.y;
        dic = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (!flag && high > 1.0f && high < 9.0f)
        {
            if (dic < 20.0f)
            {
                _light.enabled = true;
                flag = true;
                StartCoroutine(Flash());
            }
        }
        else if(flag && (high <= 1.0f || high >= 9.0f || dic > 20.0f))
        {
            _light.enabled = false;
            flag = false;
            StopCoroutine(Flash());
        }
    }

    IEnumerator<WaitForSeconds> ChangeIntensty()        //改变灯光亮度
    {
        if (source!= null && source.clip != null && !source.isPlaying)
            source.Play();
        x = Random.Range(0, 1);
        float endx = x + len;
        while (x <= endx)
        {
            x += Time.deltaTime * speed;
            _light.intensity = curve.Evaluate(x) * (startIntensity == 0?lightning:startIntensity);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _light.intensity = startIntensity;
        x = 0f;
    }

    IEnumerator<WaitForSeconds> Flash()                     //灯光闪烁
    {
        while (true)
        {
            if (Random.Range(0, 1000) < 1000 * posibility)
                StartCoroutine(ChangeIntensty());           //一直执行改变亮度的函数

            yield return new WaitForSeconds(Random.Range(len + minGapTime, len + maxGapTime));      //每次的间隔时间
        }
    }
}
