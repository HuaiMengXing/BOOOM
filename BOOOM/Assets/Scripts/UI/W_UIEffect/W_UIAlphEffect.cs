using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_UIAlphEffect : MonoBehaviour
{
    public AnimationCurve targetCurve;
    public float speed = 10f;
    public float endX = 1f;
    public bool alwaysOn = true;

    private float x = 0f;
    private bool isPlaying = false;

    private float targetY;
    private CanvasGroup canvasGroup;

    public void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        targetY = targetCurve.Evaluate(endX);
        targetY = Mathf.Clamp(targetY, 0f, 1f);
    }
    public void Update()
    {
        if (isPlaying == true && (alwaysOn || canvasGroup.alpha != targetY))
        {
            x += Time.deltaTime * speed;
            float y = targetCurve.Evaluate(x);
            canvasGroup.alpha = y;
            if (x>=endX)
            {
                isPlaying = false;
                canvasGroup.alpha = targetY;
                x = 0;
            }
        }
        else if (isPlaying == false && (alwaysOn || canvasGroup.alpha != targetY))
        {
            x += Time.deltaTime * speed;
            float y = targetCurve.Evaluate(x);
            canvasGroup.alpha = y;
            if (x >= endX)
            {
                isPlaying = false;
                canvasGroup.alpha = targetY;
                x = 0;
            }
        }
    }

}
