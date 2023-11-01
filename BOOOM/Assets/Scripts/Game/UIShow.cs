using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShow : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public float speed = 1f;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime;
            if(_canvasGroup.alpha >= 1)
            {
                _canvasGroup.alpha = 1;
                Time.timeScale = 0;
            }
                
        }            
    }
}
