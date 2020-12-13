using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerSupplyGame : MonoBehaviour
{
    private Transform HandleTF;

    private Vector3 FirstTouch;
    private Vector3 LastTouch;

    private GameObject Handle;

    private const int numLights = 8;
    private Image[] lightImages = new Image[numLights];

    private int numRotations;

    private bool check = true;

    //컬러 객체
    private Color off = new Color(69, 132, 173, 255);
    private Color on = new Color(255, 255, 0, 255);

    //발전소 미니게임 패널 게임오브젝트
    private GameObject power_supply_panel;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject power_supply;
    private AudioSource audioSource;

    void Start()
    {
        Handle = this.transform.parent.transform.GetChild(1).gameObject;
        HandleTF = Handle.transform.GetComponent<Transform>();

        power_supply_panel = this.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        mini_game = power_supply_panel.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();

        numRotations = 0;

        for (int i = 0; i < numLights; i++)
        {
            lightImages[i] = this.transform.GetChild(i+1).gameObject.GetComponent<Image>();
            lightImages[i].color = off;
        }
        audioSource = this.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FirstTouch.x = Input.mousePosition.x;
            FirstTouch.y = Input.mousePosition.y;
        }

        if (Input.GetMouseButton(0))
        {
            audioSource.Play();
            LastTouch.x = Input.mousePosition.x;
            LastTouch.y = Input.mousePosition.y;
            
            float angle = Mathf.Atan2(LastTouch.x - FirstTouch.x, LastTouch.y - FirstTouch.y) * Mathf.Rad2Deg;
            //Debug.Log("angle is " + angle);

            Handle.transform.rotation = Quaternion.Euler(Handle.transform.rotation.x, Handle.transform.rotation.y, -angle);
            if (angle > -20 && angle < 20)
            {
                numRotations++;
                check = false;
            }
            if (angle < -20 || angle > 20)
            {
                check = true;
            }
        }

        Quaternion target = Quaternion.LookRotation(HandleTF.position - Handle.transform.position);
        Handle.transform.rotation = Quaternion.Slerp(Handle.transform.rotation, target, Time.deltaTime * 3f);

        if(numRotations > 0 && numRotations <= numLights)
        {
            for(int  i =0; i<numRotations; i++)
            {
                if(lightImages[numRotations-1].color == on)
                {
                    Exit_Panel();
                }
                lightImages[i].color = on;
            }
        }
    }

    public void Exit_Panel()
    {
        //패널 비활성화
        power_supply_panel.SetActive(false);
        //미션 종료 이후 업데이트
        mini_game.ClearMiniGame(power_supply);
    }
}