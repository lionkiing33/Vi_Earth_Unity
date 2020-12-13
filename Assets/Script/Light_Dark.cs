using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light_Dark : MonoBehaviour
{
    public GameObject Light;

    // Start is called before the first frame update
    void Start()
    {
        Light.SetActive(false);
    }
    public void Light_Button_OnMouseDown()
    {
        //canvas를 활성화 시킨다.
        if (Light.activeSelf == false)
        {
            //불 좀 꺼줄래?
            Light.SetActive(true);
        }
        //canvas를 비활성화 시킨다.
        else if (Light.activeSelf == true)
        {
            //불 좀 켜줄래?
            Light.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
