using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    //플레이어 게임오브젝트
    public GameObject player;

    //미니게임패널 게임오브젝트 배열
    public const int numOfMiniGame = 9;
    public GameObject[] miniGame = new GameObject[numOfMiniGame];

    //조이스틱 게임오브젝트
    public GameObject joystick;

    //미션 게임오브젝트 배열
    public GameObject[] interactObject = new GameObject[numOfMiniGame];
    private InteractObject[] interactions = new InteractObject[numOfMiniGame];

    public GameObject bell;
    private InteractObject bell_interact;
    public GameObject bell_panel;
    private Bell bell_script;

    public List<GameObject> missions = new List<GameObject>();

    private double[] distances;
    private double min;
    public GameObject nearbyObject;

    //상호작용 게임오브젝트 이미지 객체
    private SpriteRenderer objImage;
    private SpriteOutline objectBorder;

    //버튼 배경 게임오브젝트 이미지 객체
    private Image btnImage;
    private Button interactButton;

    //컬러 객체
    private Color normal = new Color(255, 255, 255, 255);
    private Color yellow = new Color(255, 255, 0, 255);
    private Color gray = new Color(191, 191, 191, 255);
    private Color transparent = new Color(255, 255, 255, 0.5f);

    void Start()
    {
        for (int i = 0; i < numOfMiniGame; i++)
        {
            miniGame[i].SetActive(false);
            interactions[i] = interactObject[i].GetComponent<Interaction>().interactObject;
            if (interactions[i].isItUsable == false)
            {
                missions.Add(interactObject[i]);
            }
        }

        bell_interact = bell.GetComponent<Interaction>().interactObject;
        missions.Add(bell);

        btnImage = this.GetComponent<Image>();
        interactButton = this.GetComponent<Button>();
        ChangeButtonUI(false);
    }
    /*
    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < numOfMiniGame; i++)
        {
            miniGame[i].SetActive(false);
            interactions[i] = interactObject[i].GetComponent<Interaction>().interactObject;
            if (interactions[i].isItUsable == false)
            {
                missions.Add(interactObject[i]);
            }
        }

        btnImage = this.GetComponent<Image>();
        interactButton = this.GetComponent<Button>();
        ChangeButtonUI(false);
    }
    */

    // Update is called once per frame
    void Update()
    {
        //플레이어의 위치에 따른 미션오브젝트 리스트 내역 거리 비교 및 UI변경
        Vector3 posPlayer = player.gameObject.transform.position;
        btwPlayerAndObjects(posPlayer, missions);
    }

    public void ChangeButtonUI(bool active)
    {
        //활성화하기
        if(active)
        {
            btnImage.color = normal;
            interactButton.interactable = true;
        }
        //비활성화하기
        else
        {
            btnImage.color = transparent;
            interactButton.interactable = false;
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
    public void btwPlayerAndObjects(Vector3 posPlayer, List<GameObject> missions)
    {
        Vector3 posObject;
        min = 100.0;
        distances = new double[missions.Count];
        for (int i=0;i<missions.Count;i++)
        {
            if(missions[i].GetComponent<Interaction>().interactObject.isItUsable == false)
            {
                posObject = missions[i].gameObject.transform.position;
                double distance = calcDistance(posPlayer, posObject);
                distances[i] = distance;

                if (distances[i] > 10.0f)
                {
                    //오브젝트 태두리 설정
                    objectBorder = missions[i].GetComponent<SpriteOutline>();
                    objectBorder.enabled = false;
                    //오브젝트 컬러 설정
                    objImage = missions[i].gameObject.GetComponent<SpriteRenderer>();
                    objImage.color = normal;
                }
                else if (distances[i] <= 10.0f && distances[i] > 5.0f)
                {
                    //오브젝트 태두리 설정
                    objectBorder = missions[i].GetComponent<SpriteOutline>();
                    objectBorder.enabled = true;
                    //오브젝트 컬러 설정
                    objImage = missions[i].gameObject.GetComponent<SpriteRenderer>();
                    objImage.color = normal;
                }
                else
                {
                    //오브젝트 태두리 설정
                    objectBorder = missions[i].GetComponent<SpriteOutline>();
                    objectBorder.enabled = true;
                    if (missions[i] == bell)
                    {
                        //오브젝트 컬러 설정
                        objImage = missions[i].gameObject.GetComponent<SpriteRenderer>();
                        objImage.color = gray;
                    }
                    else
                    {
                        //오브젝트 컬러 설정
                        objImage = missions[i].gameObject.GetComponent<SpriteRenderer>();
                        objImage.color = yellow;
                    }
                        
                }

                if (min > distances[i])
                {
                    min = distances[i];
                    nearbyObject = missions[i];
                }
                if (i == missions.Count - 1)
                {
                    if (min <= 5.0f)
                    {
                        ChangeButtonUI(true);

                        InteractObject paramObject = nearbyObject.gameObject.GetComponent<Interaction>().interactObject;

                        if (interactButton.onClick != null)
                        {
                            interactButton.onClick.RemoveAllListeners();
                        }
                        interactButton.onClick.AddListener(() => InteractMissionObject(paramObject));
                    }
                    else
                    {
                        ChangeButtonUI(false);
                    }
                }
            }
            else
            {
                //오브젝트 태두리 설정
                objectBorder = missions[i].GetComponent<SpriteOutline>();
                objectBorder.enabled = false;
                //오브젝트 컬러 설정
                objImage = missions[i].gameObject.GetComponent<SpriteRenderer>();
                objImage.color = normal;
            }
        }
    }

    //근처 미션이 가능한 오브젝트 있을 경우 사용이 가능하며
    //해당 미션오브젝트의 정보를 전달받아야합니다
    //해당 미션에 해당하는 미니게임을 나타내야합니다.
    public void InteractMissionObject(InteractObject obj)
    {
        if(obj.isItUsable == false)
        {
            if (obj.objectType == InteractObject.ObjectType.BASKETBALL_HOOP)
            {
                miniGame[0].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.SAFE)
            {
                miniGame[1].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.SHELF)
            {
                miniGame[2].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.GAS_STATION)
            {
                miniGame[3].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.COIN)
            {
                miniGame[4].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.VACCINE)
            {
                miniGame[5].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.POWER_SUPPLY)
            {
                miniGame[6].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.COFFIN)
            {
                miniGame[7].SetActive(true);
            }
            else if (obj.objectType == InteractObject.ObjectType.BOOK)
            {
                miniGame[8].SetActive(true);
            }
            else if(obj.objectType == InteractObject.ObjectType.BELL)
            {
                bell_script = bell_panel.transform.GetChild(1).transform.GetChild(0).GetComponent<Bell>();
                bell_script.player = player;
                bell_panel.SetActive(true);
            }
            joystick.SetActive(false);
            ChangeButtonUI(false);
        }
    }
}