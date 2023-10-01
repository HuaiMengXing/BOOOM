using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灯光闪烁脚本
/// </summary>
public class W_LightFlash : MonoBehaviour
{
    public AnimationCurve curve;                                    //控制灯光变化的曲线
                                                                    
    public Light light => this.GetComponentInChildren<Light>();     //获取物体的Light组件

    public float duration = 1f;                                     //灯光闪烁一次的时间

    [Range(0f,1000f)]
    public float minGapTime = 5f;                                   //闪烁的最小间隔时间

    [Range(0f,1000f)]
    public float maxGapTime = 3f;                                   //闪烁的最大间隔时间

    private float startIntensity = 10f;                             //Light的起使亮度

    private float x = 0f;

    public void Start()
    {
        if (minGapTime < duration) minGapTime = duration;           //对非法数值进行处理(可能需要优化)
        if (maxGapTime < minGapTime) maxGapTime = minGapTime;
        startIntensity = light.intensity;
        StartCoroutine(Flash());                                    
    }

    IEnumerator<WaitForSeconds> ChangeIntensty()        //改变灯光亮度
    {
        while (x <= 1)
        {
            x += Time.deltaTime / duration;
            light.intensity = curve.Evaluate(x) * startIntensity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        x = 0f;
    }

    IEnumerator<WaitForSeconds> Flash()                 //灯光闪烁
    {
        while (true)                            
        {
            StartCoroutine(ChangeIntensty());           //一直执行改变亮度的函数
            yield return new WaitForSeconds(Random.Range(minGapTime, maxGapTime));      //每次的间隔时间
        }
    }
}
