using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƹ���˸�ű�
/// </summary>
public class W_LightFlash : MonoBehaviour
{
    public AnimationCurve curve;                                    //���Ƶƹ�仯������
                                                                    
    public Light light => this.GetComponentInChildren<Light>();     //��ȡ�����Light���

    public float duration = 1f;                                     //�ƹ���˸һ�ε�ʱ��

    [Range(0f,1000f)]
    public float minGapTime = 5f;                                   //��˸����С���ʱ��

    [Range(0f,1000f)]
    public float maxGapTime = 3f;                                   //��˸�������ʱ��

    private float startIntensity = 10f;                             //Light����ʹ����

    private float x = 0f;

    public void Start()
    {
        if (minGapTime < duration) minGapTime = duration;           //�ԷǷ���ֵ���д���(������Ҫ�Ż�)
        if (maxGapTime < minGapTime) maxGapTime = minGapTime;
        startIntensity = light.intensity;
        StartCoroutine(Flash());                                    
    }

    IEnumerator<WaitForSeconds> ChangeIntensty()        //�ı�ƹ�����
    {
        while (x <= 1)
        {
            x += Time.deltaTime / duration;
            light.intensity = curve.Evaluate(x) * startIntensity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        x = 0f;
    }

    IEnumerator<WaitForSeconds> Flash()                 //�ƹ���˸
    {
        while (true)                            
        {
            StartCoroutine(ChangeIntensty());           //һֱִ�иı����ȵĺ���
            yield return new WaitForSeconds(Random.Range(minGapTime, maxGapTime));      //ÿ�εļ��ʱ��
        }
    }
}
