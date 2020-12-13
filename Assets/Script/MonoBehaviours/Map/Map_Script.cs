using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Map_Script : MonoBehaviour
{
    public GameObject miniMap;
    public GameObject player;
    public GameObject miniPlayer;

    void Start()
    {
        //미니맵 비활성화
        miniMap.SetActive(false);
        //기존 플레이어의 좌표값을 저장합니다.
        //pos = player.transform.position;
        //해당 미니맵 내 플레이어의 위치를 표현해 주는 미니플레이어의 좌표를 기존 플레이어의 위치와 동일하게 설정합니다.

    }

    void Update()
    {

        //현재 맵의 플레이어의 위치입니다.
        Vector3 Current_Player_Position = player.transform.localPosition;

        //미니맵 내의 플레이어의 위치를 생성합니다.
        Vector3 Minimap_Player_Position = Vector3.zero;

        //미니맵 내의 플레이어의 위치를 기존 맵의 플레이어의 위치와 비율을 조정합니다.
        Minimap_Player_Position.x = Current_Player_Position.x * 10;
        Minimap_Player_Position.y = Current_Player_Position.y * 10;
        miniPlayer.transform.localPosition = Minimap_Player_Position;

    }

    public void Map_Button_OnMouseDown()
    {
        //미니맵을 활성화 시킨다.
        if (miniMap.activeSelf == false)
        {
            //미니맵을 활성화합니다.
            miniMap.SetActive(true);
            miniPlayer.SetActive(true);
        }
        //미니맵이 비활성화 시킨다.
        else if (miniMap.activeSelf == true)
        {
            //미니맵을 비활성화합니다.
            miniMap.SetActive(false);
            miniPlayer.SetActive(false);
        }
    }
}
