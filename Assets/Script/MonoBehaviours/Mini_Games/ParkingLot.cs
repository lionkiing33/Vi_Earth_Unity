using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParkingLot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //주차장 미니게임 패널 게임오브젝트
    private GameObject parking_lot;
    //드럼통 내 오일 용량 게임오브젝트
    private GameObject oil_mask;
    //드럼통 내 오일 용량 이미지
    private Image oil_quantity;
    //버튼이 눌러지고 있는지 확인하는 변수
    private bool isBtnDown;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject gas_station;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //주차장 미니게임 패널 게임오브젝트 초기화
        parking_lot = this.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        //드럼통 내 오일 용량 게임오브젝트 초기화
        oil_mask = this.transform.parent.transform.parent.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        //드럼통 내 오일 용량 이미지 초기화
        oil_quantity = oil_mask.GetComponent<Image>();
        //드럼통 내 오일 용량 0으로 초기화
        oil_quantity.fillAmount = 0;
        //버튼이 안눌러졌다고 설정
        isBtnDown = false;
        mini_game = parking_lot.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //미션 완료
        if(oil_quantity.fillAmount == 1)
        {
            Exit_Panel();
        }
        //미션 진행 파악
        else if(isBtnDown)
        {
            audioSource.Play();
            //오일 용량 0.0005f 만큼 증가
            oil_quantity.fillAmount += 0.0005f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //버튼 클릭 ON
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //버튼 클릭 OFF
        isBtnDown = false;
    }

    public void Exit_Panel()
    {
        //패널 비활성화
        parking_lot.SetActive(false);
        //미션 종료 이후 업데이트
        mini_game.ClearMiniGame(gas_station);
        if(gas_station.min < gas_station.max)
        {
            //드럼통 내 오일 용량 0으로 초기화
            oil_quantity.fillAmount = 0;
            //버튼이 안눌러졌다고 설정
            isBtnDown = false;
        }
    }
}
