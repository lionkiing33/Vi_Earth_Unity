using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hospital : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //병원 미니게임 패널 게임오브젝트
    private GameObject hospital;

    //백신 용량(위/중간 위/중간 아래/아래) 이미지 배열 생성
    private const int numOfVaccine = 4;
    private Image[] vaccine_quantity = new Image[numOfVaccine];

    //수사기 용량(위/중간/아래) 이미지 배열 생성
    private const int numOfSyringe = 3;
    private Image[] syringe_quantity = new Image[numOfSyringe];

    //수사기 고무마개 게임오브젝트
    private GameObject syringe_rubber;
    //수사기 고무마개 위치
    private Vector3 rubber_position;

    //수사기 누름대 게임오브젝트
    private GameObject syringe_pull;
    //수사기 누름대 위치
    private Vector3 pull_position;
    //수사기 누름대 크기
    private RectTransform pull_size;
    //수사기 누름대 높이
    private float height;
    //버튼이 눌러지고 있는지 확인하는 변수
    private bool isBtnDown;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject syringe;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //병원 미니게임 패널 게임오브젝트 초기화
        hospital = this.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        //백신 용량(위/중간 위/중간 아래/아래) 이미지 배열 초기화
        for (int i = 0; i < numOfVaccine; i++)
        {
            vaccine_quantity[i] = this.transform.parent.transform.parent.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>();
            vaccine_quantity[i].fillAmount = 0;
        }
        //수사기 용량(위/중간/아래) 이미지 배열 초기화
        for (int i = 0; i < numOfSyringe; i++)
        {
            syringe_quantity[i] = this.transform.parent.transform.parent.transform.GetChild(i + 1).transform.GetChild(0).gameObject.GetComponent<Image>();
            syringe_quantity[i].fillAmount = 0;
        }
        //수사기 고무마개 게임오브젝트, 위치 초기화
        syringe_rubber = this.transform.parent.transform.parent.transform.GetChild(4).gameObject;
        rubber_position = syringe_rubber.transform.localPosition;
        rubber_position.y = 52.5f;
        syringe_rubber.transform.localPosition = rubber_position;
        //수사기 누름대 게임오브젝트, 위치, 크기 초기화
        syringe_pull = this.transform.parent.transform.parent.transform.GetChild(5).gameObject;
        pull_size = syringe_pull.GetComponent<RectTransform>();
        height = 0.0f;
        pull_size.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        pull_position = syringe_pull.transform.localPosition;
        pull_position.y = -308.5f;
        syringe_pull.transform.localPosition = pull_position;
        //버튼이 안눌러졌다고 설정
        isBtnDown = false;
        mini_game = hospital.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
        audioSource = this.GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (vaccine_quantity[numOfVaccine-1].fillAmount == 1 && syringe_quantity[numOfSyringe-1].fillAmount == 1)
        {
            Exit_Panel();
        }
        else if(isBtnDown)
        {
            Decrease_Vaccine_Quantity(vaccine_quantity);
            Increase_Syringe_Quantity(syringe_quantity);
        }
    }

    public void Decrease_Vaccine_Quantity(Image[] vaccine)
    {
        for (int i = 0; i < vaccine.Length; i++)
        {
            if (vaccine[i].fillAmount < 1)
            {
                if (vaccine[i] == vaccine[0])
                {
                    vaccine[0].fillAmount += 0.00649f;
                    break;
                }
                else if (vaccine[i] == vaccine[1])
                {
                    vaccine[1].fillAmount += 0.00187f;
                    break;
                }
                else if (vaccine[i] == vaccine[2])
                {
                    vaccine[2].fillAmount += 0.00378f;
                    break;
                }
                else if (vaccine[i] == vaccine[3])
                {
                    vaccine[3].fillAmount += 0.02203f;
                    break;
                }
            }
        }
    }

    public void Increase_Syringe_Quantity(Image[] syringe)
    {
        for (int i = 0; i < syringe.Length; i++)
        {
            if (syringe[i].fillAmount < 1)
            {
                if(syringe[i] == syringe[0])
                {
                    syringe[0].fillAmount += 0.33333f;
                    break;
                }
                else if (syringe[i] == syringe[1])
                {
                    syringe[1].fillAmount += 0.04098f;
                    break;
                }
                else if (syringe[i] == syringe[2])
                {
                    syringe[2].fillAmount += 0.00103f;
                    if(rubber_position.y != (-262.57f))
                    {
                        rubber_position.y -= 0.325f;
                        syringe_rubber.transform.localPosition = rubber_position;

                        height += 0.2f;
                        pull_size.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                        pull_position.y -= 0.1f;
                        syringe_pull.transform.localPosition = pull_position;
                    }
                    break;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnDown = false;
    }

    public void Exit_Panel()
    {
        //패널 비활성화
        hospital.SetActive(false);
        //미션 종료 이후 업데이트
        mini_game.ClearMiniGame(syringe);
        if (syringe.min < syringe.max)
        {
            //백신 용량(위/중간 위/중간 아래/아래) 이미지 배열 초기화
            for (int i = 0; i < numOfVaccine; i++)
            {
                vaccine_quantity[i].fillAmount = 0;
            }
            //수사기 용량(위/중간/아래) 이미지 배열 초기화
            for (int i = 0; i < numOfSyringe; i++)
            {
                syringe_quantity[i].fillAmount = 0;
            }
            //수사기 고무마개 게임오브젝트, 위치 초기화
            rubber_position = syringe_rubber.transform.localPosition;
            rubber_position.y = 52.5f;
            syringe_rubber.transform.localPosition = rubber_position;
            //수사기 누름대 게임오브젝트, 위치, 크기 초기화
            pull_size = syringe_pull.GetComponent<RectTransform>();
            height = 0.0f;
            pull_size.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            pull_position = syringe_pull.transform.localPosition;
            pull_position.y = -308.5f;
            syringe_pull.transform.localPosition = pull_position;
            //버튼이 안눌러졌다고 설정
            isBtnDown = false;
        }
    }
}
