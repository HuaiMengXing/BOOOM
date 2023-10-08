using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Transform eatPos;
    public float eatSpeed;
    public TextAsset text;

    private bool one = true;
    void Update()
    {
        if(Player.Instance.death)
        {
            if(text != null && one)
            {
                one = false;
                DialogSystem.Instance.GetTextFromFile(text);
                DialogSystem.Instance.gameObject.SetActive(true);
            }
            transform.position = Vector3.Lerp(transform.position, eatPos.position, eatSpeed * Time.deltaTime);   
        }
    }
}
