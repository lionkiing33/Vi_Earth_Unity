using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SabotageMap : MonoBehaviour
{
    public GameObject player;
    private GameObject miniPlayer;
    private const int numOfSabotage = 6;
    private Button[] sabotageButton = new Button[numOfSabotage];
    private Image[] sabotageButtonImage = new Image[numOfSabotage];
    private Text[] sabotageButtonText = new Text[numOfSabotage];
    private bool[] isItProcessing = new bool[numOfSabotage];
    private float[] currentCoolTime = new float[numOfSabotage]; //남은 쿨타임을 추적 할 변수
    private IEnumerator[] coolTimeImage = new IEnumerator[numOfSabotage];
    private IEnumerator[] coolTimeText = new IEnumerator[numOfSabotage];

    public const int numOfDoor = 14;
    public GameObject[] door = new GameObject[numOfDoor];
    private Vector3[] posDoor = new Vector3[numOfDoor];

    public FadeInOut fadeCamera;
    public GameObject lightOut;
    private float coolTime = 10f;
    private GameObject map;

    InteractButton interact;
    public GameObject lamp;

    public int index;
    public bool isItStarted = false;

    /*
    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < numOfSabotage; i++)
        {
            if (i < 4)
            {
                sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).GetComponent<Button>();
                sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            }
            else if (i == 4)
            {
                sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Button>();
                sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
            }
            else if (i == 5)
            {
                sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Button>();
                sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            }
            //버튼 활성화
            sabotageButton[i].enabled = true;
            //버튼 이미지 보이게함
            sabotageButtonImage[i].fillAmount = 0;
            //버튼 타이머 안보이게함
            sabotageButtonText[i].text = null;
            //코루틴 진행 X
            isItProcessing[i] = false;
            //타이머 시간 0으로 초기화
            currentCoolTime[i] = 0f;

            coolTimeImage[i] = Cooltime(i);
            coolTimeText[i] = CoolTimeCounter(i);
        }

        for(int i=0;i<numOfDoor;i++)
        {
            posDoor[i] = door[i].transform.position;
        }
        map = this.transform.GetChild(0).gameObject;
        map.SetActive(false);
        miniPlayer = map.transform.GetChild(0).gameObject;
        lightOut.SetActive(false);
    }
    */

    void Update()
    {
        if (isItStarted)
        {
            for (int i = 0; i < numOfSabotage; i++)
            {
                if (i < 4)
                {
                    sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).GetComponent<Button>();
                    sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                    sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i + 1).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
                }
                else if (i == 4)
                {
                    sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).GetComponent<Button>();
                    sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
                    sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
                }
                else if (i == 5)
                {
                    sabotageButton[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<Button>();
                    sabotageButtonImage[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                    sabotageButtonText[i] = this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
                }
                //버튼 활성화
                sabotageButton[i].enabled = true;
                //버튼 이미지 보이게함
                sabotageButtonImage[i].fillAmount = 0;
                //버튼 타이머 안보이게함
                sabotageButtonText[i].text = null;
                //코루틴 진행 X
                isItProcessing[i] = false;
                //타이머 시간 0으로 초기화
                currentCoolTime[i] = 0f;

                coolTimeImage[i] = Cooltime(i);
                coolTimeText[i] = CoolTimeCounter(i);
            }

            for (int i = 0; i < numOfDoor; i++)
            {
                posDoor[i] = door[i].transform.position;
            }
            map = this.transform.GetChild(0).gameObject;
            map.SetActive(false);
            miniPlayer = map.transform.GetChild(0).gameObject;
            lightOut.SetActive(false);
            isItStarted = false;
        }
        else
        {
            if(player != null)
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
        }
    }

    public void Close_The_Door(int index, bool isItOpened)
    {
        if (index == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                posDoor[i] = door[i].transform.position;
            }
            if (isItOpened)
            {
                posDoor[0].x += 2f;
                posDoor[1].x -= 2f;
                posDoor[2].y -= 2f;
                posDoor[3].y += 2f;
            }
            else
            {
                posDoor[0].x -= 2f;
                posDoor[1].x += 2f;
                posDoor[2].y += 2f;
                posDoor[3].y -= 2f;
            }
            for (int i = 0; i < 4; i++)
            {
                door[i].transform.position = posDoor[i];
            }
        }
        else if (index == 1)
        {
            for (int i = 4; i < 10; i++)
            {
                posDoor[i] = door[i].transform.position;
            }
            if (isItOpened)
            {
                posDoor[4].y -= 1.5f;
                posDoor[5].y += 1.5f;
                posDoor[6].y -= 1.5f;
                posDoor[7].y += 1.5f;
                posDoor[8].y -= 1.5f;
                posDoor[9].y += 1.5f;
            }
            else
            {
                posDoor[4].y += 1.5f;
                posDoor[5].y -= 1.5f;
                posDoor[6].y += 1.5f;
                posDoor[7].y -= 1.5f;
                posDoor[8].y += 1.5f;
                posDoor[9].y -= 1.5f;
            }
            for (int i = 4; i < 10; i++)
            {
                door[i].transform.position = posDoor[i];
            }
        }
        else if (index == 2)
        {
            posDoor[10] = door[10].transform.position;
            posDoor[11] = door[11].transform.position;
            if (isItOpened)
            {
                posDoor[10].y -= 2f;
                posDoor[11].y += 2f;
            }
            else
            {
                posDoor[10].y += 2f;
                posDoor[11].y -= 2f;
            }
            door[10].transform.position = posDoor[10];
            door[11].transform.position = posDoor[11];
        }
        else if (index == 3)
        {
            posDoor[12] = door[12].transform.position;
            posDoor[13] = door[13].transform.position;
            if (isItOpened)
            {
                posDoor[12].x += 2.5f;
                posDoor[13].x -= 2.5f;
            }
            else
            {
                posDoor[12].x -= 2.5f;
                posDoor[13].x += 2.5f;
            }
            door[12].transform.position = posDoor[12];
            door[13].transform.position = posDoor[13];
        }
    }

    public void Start_Nuclear_Sabotage()
    {
        StartCoroutine(fadeCamera.coroutine);
    }

    public void No_Light_Sabotage()
    {
        lightOut.SetActive(true);
    }

    public void Click_Sabotage(int index)
    {
        SabotageIndex SabotageIndex = new SabotageIndex();
        SabotageIndex.index = index;

        string data = JsonUtility.ToJson(SabotageIndex, prettyPrint: true);
        SocketManger.Socket.Emit("SendSabotageIndex", data);
        Debug.Log("사보타지 데이터 전달");

        if (index < 4)
        {
            Close_The_Door(index, true);
            sabotageButtonImage[index].fillAmount = 1; //스킬 버튼을 가림
            sabotageButton[index].enabled = false;

            sabotageButtonImage[4].fillAmount = 1; //스킬 버튼을 가림
            sabotageButton[4].enabled = false;

            sabotageButtonImage[5].fillAmount = 1; //스킬 버튼을 가림
            sabotageButton[5].enabled = false;

            StartCoroutine(coolTimeImage[index]);

            currentCoolTime[index] = coolTime;
            sabotageButtonText[index].text = "" + currentCoolTime[index];

            StartCoroutine(coolTimeText[index]);

            isItProcessing[index] = true; //스킬을 사용하면 사용할 수 없는 상태로 바꿈
        }
        else
        {
            if (index == 5)
            {
                Start_Nuclear_Sabotage();
            }
            else if (index == 4)
            {
                No_Light_Sabotage();
            }
            for (int i = 0; i < numOfSabotage; i++)
            {
                if (i < 4 && isItProcessing[i] == true)
                {
                    Debug.Log("코루틴 " + i + "번째 강제종료!");
                    StopCoroutine(coolTimeImage[i]);
                    StopCoroutine(coolTimeText[i]);
                }
                sabotageButtonImage[i].fillAmount = 1; //스킬 버튼을 가림
                sabotageButton[i].enabled = false;
                sabotageButtonText[i].text = null;
            }
        }
    }

    IEnumerator Cooltime(int index)
    {
        Debug.Log("쿨타임 코루틴 " + index + "번째 시작!");
        while (sabotageButtonImage[index].fillAmount > 0)
        {
            sabotageButtonImage[index].fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }
        isItProcessing[index] = false; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈
        Debug.Log("쿨타임 코루틴 " + index + "번째 끝!");
        yield break;
    }

    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator CoolTimeCounter(int index)
    {
        Debug.Log("쿨타임 코루틴 타이머 " + index + "번째 시작!");
        while (currentCoolTime[index] > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime[index] -= 1.0f;
            sabotageButtonText[index].text = "" + currentCoolTime[index];

            if (Get_Minimum_Time() == 5.0f)
            {
                Close_The_Door(index, false);
                sabotageButtonImage[4].fillAmount = 0; //스킬 버튼을 가림
                sabotageButton[4].enabled = true;

                sabotageButtonImage[5].fillAmount = 0; //스킬 버튼을 가림
                sabotageButton[5].enabled = true;
            }
        }
        //버튼 활성화
        sabotageButton[index].enabled = true;
        //버튼 이미지 보이게함
        sabotageButtonImage[index].fillAmount = 0;
        //버튼 타이머 안보이게함
        sabotageButtonText[index].text = null;
        //코루틴 진행 X
        isItProcessing[index] = false;

        coolTimeImage[index] = Cooltime(index);
        coolTimeText[index] = CoolTimeCounter(index);

        Debug.Log("쿨타임 코루틴 타이머 " + index + "번째 끝!");
        yield break;
    }

    public float Get_Minimum_Time()
    {
        float max = currentCoolTime[0];

        for (int i = 0; i < numOfSabotage; i++)
        {
            if (currentCoolTime[i] > max)
            {
                max = currentCoolTime[i];
            }
        }
        return max;
    }

    //맵 종료하는기능
    public void Exit_Map()
    {
        if (map.activeSelf == false)
        {
            map.SetActive(true);
        }
        else
        {
            map.SetActive(false);
        }
    }
}

public class SabotageIndex
{
    public int index;
}