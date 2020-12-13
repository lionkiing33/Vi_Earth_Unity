using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VentButton : MonoBehaviour
{
    //플레이어 게임오브젝트
    public GameObject player;
    private bool isItVented;
    private Vector3 posPlayer;
    private PlayerControllerScript controll;
    private SpriteRenderer playerImage;
    private Animator animator;

    //벤트 게임오브젝트 배열
    public const int numOfVent = 9;
    public GameObject[] vent = new GameObject[numOfVent];

    //조이스틱 게임오브젝트
    public GameObject joystick;

    //벤트 오브젝트 배열
    public GameObject arrowArray;
    private VentObject[] vent_interactions = new VentObject[numOfVent];

    //화살표 게임오브젝트 배열
    private const int numOfArrow = 8;
    private GameObject[] Arrow = new GameObject[numOfArrow];
    private Button[] arrowButton = new Button[numOfArrow];

    //벤트와 플레이어 사이 거리 double 배열
    private double[] distances;
    private double min;

    //플레이어와 제일 가까운 벤트 게임오브젝트
    public GameObject nearbyObject;

    //벤트 게임오브젝트 이미지 객체
    private SpriteRenderer ventImage;
    private SpriteOutline ventBorder;

    //버튼 배경 게임오브젝트 이미지 객체
    private Image ventButtonImage;
    private Button ventButton;

    //컬러 객체
    private Color normal = new Color(255, 255, 255, 255);
    private Color red = new Color(255, 0, 0, 255);
    private Color transparent = new Color(255, 255, 255, 0.5f);
    private Color none = new Color(255, 255, 255, 0);

    private IEnumerator coroutine;

    public bool isItStarted = false;

    /*
    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < numOfVent; i++)
        {
            vent_interactions[i] = vent[i].GetComponent<Vent_InteractObject>().ventObject;
        }
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i] = arrowArray.transform.GetChild(i).gameObject;
            Arrow[i].SetActive(false);
            arrowButton[i] = Arrow[i].GetComponent<Button>();
        }

        ventButtonImage = this.GetComponent<Image>();
        ventButton = this.GetComponent<Button>();
        ChangeButtonUI(false);
        isItVented = false;

        animator = player.GetComponent<Animator>();
        controll = player.GetComponent<PlayerControllerScript>();
        playerImage = player.GetComponent<SpriteRenderer>();
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (isItStarted)
        {
            for (int i = 0; i < numOfVent; i++)
            {
                vent_interactions[i] = vent[i].GetComponent<Vent_InteractObject>().ventObject;
            }
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i] = arrowArray.transform.GetChild(i).gameObject;
                Arrow[i].SetActive(false);
                arrowButton[i] = Arrow[i].GetComponent<Button>();
            }

            ventButtonImage = this.GetComponent<Image>();
            ventButton = this.GetComponent<Button>();
            ChangeButtonUI(false);
            isItVented = false;

            animator = player.GetComponent<Animator>();
            controll = player.GetComponent<PlayerControllerScript>();
            playerImage = player.GetComponent<SpriteRenderer>();

            isItStarted = false;
        }
        else
        {
            //플레이어의 위치에 따른 미션오브젝트 리스트 내역 거리 비교 및 UI변경
            posPlayer = player.gameObject.transform.position;
            btwPlayerAndVent(posPlayer, vent);
        }
    }

    public void ChangeButtonUI(bool active)
    {
        //활성화하기
        if (active)
        {
            ventButtonImage.color = normal;
            ventButton.interactable = true;
        }
        //비활성화하기
        else
        {
            ventButtonImage.color = transparent;
            ventButton.interactable = false;
        }
    }

    //플레이어와 상호작용 오브젝트 사이의 거리 계산하는 메소드
    public double calcDistance(Vector3 posPlayer, Vector3 posObject)
    {
        double btwX = (posPlayer.x - posObject.x) * (posPlayer.x - posObject.x);
        double btwY = (posPlayer.y - posObject.y) * (posPlayer.y - posObject.y);
        double distance = Math.Sqrt(btwX + btwY);
        return distance;
    }

    //상호작용 객체(배열)들과 플레이어 객체 사이의 거리에 따른 UI변경 메소드
    public void btwPlayerAndVent(Vector3 posPlayer, GameObject[] vents)
    {
        Vector3 posVent;
        min = 100.0;
        distances = new double[numOfVent];
        for (int i = 0; i < numOfVent; i++)
        {
            posVent = vent_interactions[i].location;
            double distance = calcDistance(posPlayer, posVent);
            distances[i] = distance;

            if (distances[i] > 20.0f)
            {
                //오브젝트 태두리 설정
                ventBorder = vents[i].GetComponent<SpriteOutline>();
                ventBorder.enabled = false;
                //오브젝트 컬러 설정
                ventImage = vents[i].gameObject.GetComponent<SpriteRenderer>();
                ventImage.color = normal;
            }
            else if (distances[i] <= 20.0f && distances[i] > 15.0f)
            {
                //오브젝트 태두리 설정
                ventBorder = vents[i].GetComponent<SpriteOutline>();
                ventBorder.enabled = true;
                //오브젝트 컬러 설정
                ventImage = vents[i].gameObject.GetComponent<SpriteRenderer>();
                ventImage.color = normal;
            }
            else
            {
                //오브젝트 태두리 설정
                ventBorder = vents[i].GetComponent<SpriteOutline>();
                ventBorder.enabled = true;
                //오브젝트 컬러 설정
                ventImage = vents[i].gameObject.GetComponent<SpriteRenderer>();
                ventImage.color = red;
                ChangeButtonUI(true);
            }

            if (min > distances[i])
            {
                min = distances[i];
                nearbyObject = vents[i];
            }
            if (i == numOfVent - 1)
            {
                if (min <= 15.0f)
                {
                    ChangeButtonUI(true);

                    VentObject paramObject = nearbyObject.gameObject.GetComponent<Vent_InteractObject>().ventObject;

                    if (ventButton.onClick != null)
                    {
                        ventButton.onClick.RemoveAllListeners();
                    }
                    ventButton.onClick.AddListener(() => Use_Vent(paramObject));
                }
                else
                {
                    ChangeButtonUI(false);
                }
            }
        }
    }

    public void Arrow_Up_Function(int index)
    {
        //index == 7
        //공동묘지에서 서점
        player.transform.localPosition = vent_interactions[8].location;
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[3].SetActive(true);
        Arrow[4].SetActive(true);
        //return 8;
    }

    public void Arrow_Up_Right_Function(int index)
    {
        //index == 7
        //공동묘지에서 병원
        player.transform.localPosition = vent_interactions[6].location;
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[5].SetActive(true);
        Arrow[7].SetActive(true);
        //return 6;
    }

    public void Arrow_Right_Function(int index)
    {
        if (index == 0)
        {
            //농구장에서 호수 상단
            //return 1;
            player.transform.localPosition = vent_interactions[1].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(1));
            Arrow[6].SetActive(true);
            if (arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(1));
        }
        else if (index == 1)
        {
            //호수 상단에서 경비실
            //return 2;
            player.transform.localPosition = vent_interactions[2].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[6].SetActive(true);
            if (arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(2));
        }
        else if (index == 4)
        {
            //호수 하단에서 마트
            //return 3;
            player.transform.localPosition = vent_interactions[3].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[6].SetActive(true);
            if (arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(3));
        }
        else if (index == 5)
        {
            //주차장에서 호수 하단
            //return 4;
            player.transform.localPosition = vent_interactions[4].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(4));
            Arrow[6].SetActive(true);
            if (arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(4));
        }
    }

    public void Arrow_Down_Right_Function(int index)
    {
        //index == 8
        //서점에서 병원
        player.transform.localPosition = vent_interactions[6].location;
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[5].SetActive(true);
        Arrow[7].SetActive(true);
        //return 6;
    }

    public void Arrow_Down_Function(int index)
    {
        //index == 8
        //서점에서 공동묘지
        player.transform.localPosition = vent_interactions[7].location;
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[0].SetActive(true);
        Arrow[1].SetActive(true);
        //return 7;
    }

    public void Arrow_Down_Left_Function(int index)
    {
        //index == 6
        //병원에서 공동묘지
        player.transform.localPosition = vent_interactions[7].location;
        for (int i = 0; i < numOfArrow; i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[0].SetActive(true);
        Arrow[1].SetActive(true);
        //return 7;
    }

    public void Arrow_Left_Function(int index)
    {
        if (index == 1)
        {
            //호수 상단에서 농구장
            //return 0;
            player.transform.localPosition = vent_interactions[0].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(0));
        }
        else if (index == 2)
        {
            //경비실에서 호수 상단
            //return 1;
            player.transform.localPosition = vent_interactions[1].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(1));
            Arrow[6].SetActive(true);
            if(arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(1));
        }
        else if (index == 3)
        {
            //마트에서 호수 하단
            //eturn 4;
            player.transform.localPosition = vent_interactions[4].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(4));
            Arrow[6].SetActive(true);
            if (arrowButton[6].onClick != null)
            {
                arrowButton[6].onClick.RemoveAllListeners();
            }
            arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(4));
        }
        else if (index == 4)
        {
            //호수 하단에서 주차장
            //return 5;
            player.transform.localPosition = vent_interactions[5].location;
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            Arrow[2].SetActive(true);
            if (arrowButton[2].onClick != null)
            {
                arrowButton[2].onClick.RemoveAllListeners();
            }
            arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(5));
        }
    }

    public void Arrow_Up_Left_Function(int index)
    {
        //index == 6
        //병원에서 서점
        player.transform.localPosition = vent_interactions[8].location;
        for(int i=0;i<numOfArrow;i++)
        {
            Arrow[i].SetActive(false);
        }
        Arrow[3].SetActive(true);
        Arrow[4].SetActive(true);
        //return 8;
    }
    //플레이어가 벤트를 타서 사라지게 해줌
    //위치에따라 화살표를 보여줘야함
    //나올때 화살표를 안보여주고
    //플레이어가 벤트에서 나와 다시 보여줘야함
    public void Use_Vent(VentObject obj)
    {
        posPlayer = nearbyObject.transform.localPosition;
        player.transform.localPosition = posPlayer;
        if (isItVented)
        {
            joystick.SetActive(true);
            ChangeButtonUI(true);
            for (int i = 0; i < numOfArrow; i++)
            {
                Arrow[i].SetActive(false);
            }
            controll.ventState = "Down To Up";
            coroutine = VentState(controll.ventState);
            StartCoroutine(coroutine);
            isItVented = false;
        }
        else
        {
            joystick.SetActive(false);
            ChangeButtonUI(false);
            if (obj.objectType == VentObject.ObjectType.Basketball)
            {
                Arrow[2].SetActive(true);
                if (arrowButton[2].onClick != null)
                {
                    arrowButton[2].onClick.RemoveAllListeners();
                }
                arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(0));
            }
            else if (obj.objectType == VentObject.ObjectType.LakeUpper)
            {
                Arrow[2].SetActive(true);
                Arrow[6].SetActive(true);

                if (arrowButton[2].onClick != null)
                {
                    arrowButton[2].onClick.RemoveAllListeners();
                }
                arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(1));

                if (arrowButton[6].onClick != null)
                {
                    arrowButton[6].onClick.RemoveAllListeners();
                }
                arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(1));
            }
            else if (obj.objectType == VentObject.ObjectType.Security)
            {
                Arrow[6].SetActive(true);
                if (arrowButton[6].onClick != null)
                {
                    arrowButton[6].onClick.RemoveAllListeners();
                }
                arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(2));
            }
            else if (obj.objectType == VentObject.ObjectType.Mart)
            {
                Arrow[6].SetActive(true);
                if (arrowButton[6].onClick != null)
                {
                    arrowButton[6].onClick.RemoveAllListeners();
                }
                arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(3));
            }
            else if (obj.objectType == VentObject.ObjectType.LakeLower)
            {
                Arrow[2].SetActive(true);
                Arrow[6].SetActive(true);

                if (arrowButton[2].onClick != null)
                {
                    arrowButton[2].onClick.RemoveAllListeners();
                }
                arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(4));
                if (arrowButton[6].onClick != null)
                {
                    arrowButton[6].onClick.RemoveAllListeners();
                }
                arrowButton[6].onClick.AddListener(() => Arrow_Left_Function(4));
            }
            else if (obj.objectType == VentObject.ObjectType.ParkingLot)
            {
                Arrow[2].SetActive(true);
                if (arrowButton[2].onClick != null)
                {
                    arrowButton[2].onClick.RemoveAllListeners();
                }
                arrowButton[2].onClick.AddListener(() => Arrow_Right_Function(5));
            }
            else if (obj.objectType == VentObject.ObjectType.Hospital)
            {
                Arrow[5].SetActive(true);
                Arrow[7].SetActive(true);
            }
            else if (obj.objectType == VentObject.ObjectType.Cemetery)
            {
                Arrow[0].SetActive(true);
                Arrow[1].SetActive(true);
            }
            else if (obj.objectType == VentObject.ObjectType.Library)
            {
                Arrow[3].SetActive(true);
                Arrow[4].SetActive(true);
            }
            controll.ventState = "Up To Down";
            coroutine = VentState(controll.ventState);
            StartCoroutine(coroutine);
            isItVented = true;
        }
    }

    IEnumerator VentState(string state)
    {
        if (state == "Up To Down")
        {
            animator.SetInteger("AnimationState", 4);
            while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                Debug.Log("에니메이션1");
                yield return null;
            }
            playerImage.color = new Color(255, 255, 255, 0);
        }
        else if (state == "Down To Up")
        {
            playerImage.color = new Color(255, 255, 255, 255);
            animator.SetInteger("AnimationState", 5);
            while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                Debug.Log("에니메이션2");
                yield return null;
            }
            controll.ventState = "Standing";
        }
    }
}