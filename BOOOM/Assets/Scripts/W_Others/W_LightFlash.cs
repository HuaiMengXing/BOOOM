using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƹ���˸�ű�
/// </summary>
public class W_LightFlash : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float posibility = 0.4f;
    public float len = 1f;

    public AnimationCurve curve;                                    //���Ƶƹ�仯������

    public Light light => this.GetComponentInChildren<Light>();     //��ȡ�����Light���

    public float speed = 1f;
    [Range(0f, 1000f)]
    public float minGapTime = 5f;                                   //��˸����С���ʱ��

    [Range(0f, 1000f)]
    public float maxGapTime = 3f;                                   //��˸�������ʱ��

    private float startIntensity = 10f;                             //Light����ʹ����

    private float x = 0f;

    public void Start()
    {
        if (minGapTime < len) minGapTime = len;           //�ԷǷ���ֵ���д���(������Ҫ�Ż�)
        if (maxGapTime < minGapTime) maxGapTime = minGapTime;
        startIntensity = light.intensity;
        StartCoroutine(Flash());
    }

    IEnumerator<WaitForSeconds> ChangeIntensty()        //�ı�ƹ�����
    {
        x = Random.Range(0, 1);
        float endx = x + len;
        while (x <= endx)
        {
            x += Time.deltaTime * speed;
            light.intensity = curve.Evaluate(x) * startIntensity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        light.intensity = startIntensity;
        x = 0f;
    }

    IEnumerator<WaitForSeconds> Flash()                     //�ƹ���˸
    {
        while (true)
        {
            if (Random.Range(0, 1000) < 1000 * posibility)
                StartCoroutine(ChangeIntensty());           //һֱִ�иı����ȵĺ���

            yield return new WaitForSeconds(Random.Range(len + minGapTime, len + maxGapTime));      //ÿ�εļ��ʱ��
        }
    }
}
