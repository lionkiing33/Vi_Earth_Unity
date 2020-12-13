using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Security : MonoBehaviour
{
    //패스워드 4자리 배열 생성
    private const int numOfPassword = 4;
    //패스워드 4자리 이미지 배열 생성
    private Image[] pwImages = new Image[numOfPassword];

    //힌트 4자리 이미지 배열 생성
    private Image[] hintImages = new Image[numOfPassword];

    //숫자 이미지 배열 생성
    public const int numOfNumber = 11;
    public Sprite[] numImages = new Sprite[numOfNumber];
    //힌트 이미지 배열 생성
    public Sprite[] hintNumImages = new Sprite[numOfNumber-1];

    //비밀번호 숫자 랜덤 배열 생성
    private int[] random = new int[numOfPassword];

    //경비실 미니게임 패널 게임오브젝트
    private GameObject security;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject safe;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<numOfPassword;i++)
        {
            pwImages[i] = transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<Image>();
            hintImages[i] = transform.GetChild(0).transform.GetChild(i+1).gameObject.GetComponent<Image>();
        }
        Click_Reset();
        Create_New_Password();
        security = transform.parent.gameObject;
        mini_game = security.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
    }

    public void Click_Number(int index) 
    {
        for (int i = 0; i < numOfPassword; i++)
        {
            if(pwImages[i].sprite == numImages[numOfNumber - 1])
            {
                pwImages[i].sprite = numImages[index];
                break;
            }
        }
    }

    public void Click_Clear()
    {
        //4자리 숫자칸이 공백인지 확인
        //확인후 지정된 숫자와 일치하면 종료
        //아니면 오류 표시
        for(int i=0;i<numOfPassword;i++)
        {
            if (pwImages[i].sprite == numImages[numOfNumber - 1] && pwImages[i].sprite != numImages[random[i]])
            {
                break;
            }
            else if(i == numOfPassword-1 && pwImages[i].sprite == numImages[random[i]])
            {
                Exit_Panel();
            }
          
        }
    }

    public void Click_Reset()
    {
        for (int i = 0; i < numOfPassword; i++)
        {
            pwImages[i].sprite = numImages[numOfNumber - 1];
        }
    }

    public void Create_New_Password()
    {
        for (int i = 0; i < numOfPassword; i++)
        {
            random[i] = Random.Range(0, 10);
            hintImages[i].sprite = hintNumImages[random[i]];
        }
    }

    private void Exit_Panel()
    {
        //패널 비활성화
        security.SetActive(false);
        //미션 종료 이후 업데이트
        mini_game.ClearMiniGame(safe);
        if (safe.min < safe.max)
        {
            Click_Reset();
            Create_New_Password();
        }
    }
}
