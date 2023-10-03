using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_UISizeEffect : MonoBehaviour
{

    public AnimationCurve targetCurve;
    public float speed = 10f;
    public float endX = 1f;
    public bool alwaysOn = true;

    private float x = 0f;
    private bool isPlaying = false;
    private Vector3 targetScale = Vector3.one;
    private Vector3 startScale = Vector3.one;

    public void Start()
    {
        float endY = targetCurve.Evaluate(endX);
        targetScale = new Vector3(endY, endY, 1);
        startScale = this.transform.localScale;
    }
    public void Update()
    {
        if (isPlaying == true && (alwaysOn || this.transform.localScale != targetScale))
        {
            x += Time.deltaTime * speed;
            float y = targetCurve.Evaluate(x);
            if (y >= targetScale.x)
            {
                isPlaying = false;
                this.transform.localScale = targetScale;
                x = 0;
            }
            this.transform.localScale = new Vector3(y, y, 1);
        }
        else if (isPlaying == false && (alwaysOn || this.transform.localScale != startScale))
        {
            x += Time.deltaTime * speed;
            float y = targetCurve.Evaluate(x);
            if (y <= startScale.x)
            {
                isPlaying = false;
                this.transform.localScale = startScale;
                x = 0;
            }
            this.transform.localScale = new Vector3(y, y, 1);
        }
    }
}
