using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public class Data_Move_x_y
{
    public int image_count;
    public float x;
    public float y;
}
//user1
public class SocketTest : MonoBehaviour
{
    public Data_Move_x_y response_data = new Data_Move_x_y();
	public GameObject Im_win;
    public int user_type;
    PlayerControllerScript temp;
    MiniMap temp_map;
    public VentButton vent;
    Joystick joy;
    public KillButton kill;
    public SabotageMap sabotage;
    public ReportButton report;
    public InteractButton interact;
    bool user_type_check, check2, MyJob = false;
    public int image_num = 0;
    public int killed_image_num = 0;
    public float x1, y1, x2, y2, x3, y3, x4, y4, x5, y5 = 0;
    public float killed_x, killed_y = 0;
    public string MyID;
    public bool killed_check, kill_check = false;
    // public PlayerControllerScript pCS;
    //무덤 프리팹
    public GameObject graveStonePrefab;
    private GameObject graveStone;
    public KillButton killBtn;

    public GameObject[] deadPlayer = new GameObject[5];

    public GameObject cam;

    public int index;
    public bool sabo_on = false;
    public bool meet_on = false;
	public bool isItGameEnd = false;

    public string[] playerImageName = new string[5];
    public string[] playerName = new string[5];
    public string playerMeetingId = null;
    public GameObject EmergencyMeeting;

    //무덤 프리팹
    //public GameObject graveStonePrefab;

    SocketTest_player1 socketTest_player1;
    SocketTest_player2 socketTest_player2;
    SocketTest_player3 socketTest_player3;
    SocketTest_player4 socketTest_player4;
    SocketTest_player5 socketTest_player5;

    Animator animator;
    SpriteRenderer spriterenderer;
    GameObject ghost;
    Animator ghostanimator;
    SpriteRenderer ghostspriterenderer;

    void OnEnable()
    {
        SceneDeliver userinfo = GameObject.Find("SceneDeliver").GetComponent<SceneDeliver>();
        MyID = userinfo.userID;
        for (int i = 0; i < userinfo.playerInfo.USER.Length; i++)
        {
            playerName[i] = userinfo.playerInfo.USER[i].UserName;
            animator = GameObject.Find("Player" + (i + 1) + "Object").GetComponent<Animator>();
            spriterenderer = GameObject.Find("Player" + (i + 1) + "Object").GetComponent<SpriteRenderer>();
            ghost = GameObject.Find("Player" + (i + 1) + "GhostObject");
            ghostanimator = ghost.GetComponent<Animator>();
            ghostspriterenderer = ghost.GetComponent<SpriteRenderer>();
            switch (userinfo.playerInfo.USER[i].UserCharacter)
            {
                case 0:
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "Boy" + "Controller", typeof(RuntimeAnimatorController)));
                    spriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Boy" + "_Standing01");
                    ghostanimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "BoyGhost" + "Controller", typeof(RuntimeAnimatorController)));
                    ghostspriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Boy" + "_Ghost_East1");
                    playerImageName[i] = "Sprites\\Boy_Face";
                    break;
                case 1:
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "Girl" + "Controller", typeof(RuntimeAnimatorController)));
                    spriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Girl" + "_Standing01");
                    ghostanimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "GirlGhost" + "Controller", typeof(RuntimeAnimatorController)));
                    ghostspriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Girl" + "_Ghost_East1");
                    playerImageName[i] = "Sprites\\Girl_Face";
                    break;
                case 2:
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "Dracula" + "Controller", typeof(RuntimeAnimatorController)));
                    spriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Dracula" + "_Standing01");
                    ghostanimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "DraculaGhost" + "Controller", typeof(RuntimeAnimatorController)));
                    ghostspriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Dracula" + "_Ghost_East1");
                    playerImageName[i] = "Sprites\\Dracula_Face";
                    break;
                case 3:
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "Frankenstein" + "Controller", typeof(RuntimeAnimatorController)));
                    spriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Frankenstein" + "_Standing01");
                    ghostanimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "FrankensteinGhost" + "Controller", typeof(RuntimeAnimatorController)));
                    ghostspriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Frankenstein" + "_Ghost_East1");
                    playerImageName[i] = "Sprites\\Frankenstein_Face";
                    break;
                case 5:
                    animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "Mirra" + "Controller", typeof(RuntimeAnimatorController)));
                    spriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Mirra" + "_Standing01");
                    ghostanimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + "MirraGhost" + "Controller", typeof(RuntimeAnimatorController)));
                    ghostspriterenderer.sprite = Resources.Load<Sprite>("Sprites\\" + "Mirra" + "_Ghost_East1");
                    playerImageName[i] = "Sprites\\Mirra_Face";
                    break;
            }
            ghost.SetActive(false);
            if (MyID.Equals(userinfo.playerInfo.USER[i].UserID))
            {
                user_type = i + 1;
                user_type_check = true;
                if (userinfo.playerInfo.USER[i].isImposter)
                {
                    MyJob = true;
                }
            }
        }
    }
    void Start()
    {
	    SocketManger.Socket.On("GameEnd", (data) => {
            isItGameEnd = true;
			Debug.Log("게임 엔드 소켓 들어옴");
        });

        SocketManger.Socket.On("serverSendKillInfo", (data) => {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            killInfo response = JsonUtility.FromJson<killInfo>(json);
            killed_image_num = response.ImageNumber;
            killed_x = response.x;
            killed_y = response.y;
            kill_check = true;
            Debug.Log("서버에서 kill 이벤트 받음");
        });
        SocketManger.Socket.On("GetSabotageIndex", (data) => {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            Debug.Log("사보타지입니다!");
            SabotageIndeX SabotageIndex = new SabotageIndeX();
            SabotageIndex = JsonUtility.FromJson<SabotageIndeX>(json);
            index = SabotageIndex.index;
            sabo_on = true;
        });
        SocketManger.Socket.On("GetPlayerMeeting", (data) => {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            Debug.Log("미팅을 시작합니다.");
            GetPlayerMeeting getPlayerMeeting = new GetPlayerMeeting();
            getPlayerMeeting = JsonUtility.FromJson<GetPlayerMeeting>(json);
            meet_on = getPlayerMeeting.isItMeeting;
            playerMeetingId = getPlayerMeeting.playerName;
            switch (playerMeetingId)
            {
                case "Player1Object":
                    playerMeetingId = playerName[0];
                    break;
                case "Player2Object":
                    playerMeetingId = playerName[1];
                    break;
                case "Player3Object":
                    playerMeetingId = playerName[2];
                    break;
                case "Player4Object":
                    playerMeetingId = playerName[3];
                    break;
                case "Player5Object":
                    playerMeetingId = playerName[4];
                    break;
            }
            Debug.Log(meet_on);
        });
    }
    private void FixedUpdate()
    {
	    if(isItGameEnd)
        {
			 Debug.Log("소켓엔드 업데이트 들어옴");
            Im_win.SetActive(true);
            isItGameEnd = false;
			Debug.Log("소켓엔드 win 띄워줌");
        }

        if (meet_on)
        {
            EmergencyMeeting.SetActive(true);
            EmergencyMeeting.GetComponent<EmergencyMeeting>().allPlayerName = playerName;
            EmergencyMeeting.GetComponent<EmergencyMeeting>().allPlayerImageName = playerImageName;
            EmergencyMeeting.GetComponent<EmergencyMeeting>().meetingId = playerMeetingId;
            switch (user_type)
            {
                case 1:
                    EmergencyMeeting.GetComponent<EmergencyMeeting>().myPlayer = GameObject.Find("Player1Object");
                    break;
                case 2:
                    EmergencyMeeting.GetComponent<EmergencyMeeting>().myPlayer = GameObject.Find("Player2Object");
                    break;
                case 3:
                    EmergencyMeeting.GetComponent<EmergencyMeeting>().myPlayer = GameObject.Find("Player3Object");
                    break;
                case 4:
                    EmergencyMeeting.GetComponent<EmergencyMeeting>().myPlayer = GameObject.Find("Player4Object");
                    break;
                case 5:
                    EmergencyMeeting.GetComponent<EmergencyMeeting>().myPlayer = GameObject.Find("Player5Object");
                    break;
            }
            Debug.Log("투표누른사람 : " + playerMeetingId);
            meet_on = false;
        }
        if (sabo_on)
        {
            sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
            if (index < 4)
            {
                StartCoroutine(OpenCloseDoor(index, 5));
            }
            else if (index == 4)
            {
                sabotage.No_Light_Sabotage();
            }
            else if (index == 5)
            {
                sabotage.Start_Nuclear_Sabotage();
            }
            sabo_on = false;
        }
        if (kill_check)
        {
            GameObject temp_player = GameObject.Find("Player" + killed_image_num + "Object");
            Debug.Log("누가 죽었는지 확인");
            Debug.Log(temp_player);

            Debug.Log(killed_image_num + " " + user_type + "????");
            if (killed_image_num == user_type) //죽은게 자신일때
            {
                Debug.Log("내가 죽음");
                Debug.Log(temp_player);
                PlayerControllerScript.playerController_killed_check = true;

                Debug.Log(deadPlayer[killed_image_num - 1]);
                //플레이어가 죽었다고 설정
                temp_player.GetComponent<Kill_InteractObject>().killObject.IsPlayerAlive = false;

                deadPlayer[killed_image_num - 1].SetActive(true);
                deadPlayer[killed_image_num - 1].transform.localPosition = temp_player.transform.localPosition;

                interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                interact.enabled = false;
                interact.player = deadPlayer[killed_image_num - 1];
                interact.enabled = true;

                cam.GetComponent<FollowCamera>().player = deadPlayer[killed_image_num - 1];
                cam.GetComponent<FollowCamera>().check = true;
                temp_player.SetActive(false);

            }
            else
            { //죽은게 다른 상대일때
                Debug.Log("상대" + killed_image_num + "가" + "죽음ㅋㅋㅋ");
                temp_player.SetActive(false);
                //무덤 프리팹 인스턴스 생성
                graveStone = Instantiate(graveStonePrefab);
                //죽은 플레이어의 킬 인터렉트 속성 값을 무덤 오브젝트의 킬 인터렉트 속성에 저장함
                graveStone.GetComponent<Kill_InteractObject>().killObject = temp_player.GetComponent<Kill_InteractObject>().killObject;
                //무덤의 위치를 죽은 플레이어의 위치로 이동
                graveStone.transform.localPosition = new Vector3(killed_x, killed_y, 0);
                //무덤 오브젝트를 무덤 리스트에 추가함
                killBtn.graveStoneList.Add(graveStone);
            }
            kill_check = false;
        }
        if (user_type_check)
        {
            joy = GameObject.Find("Joystick").GetComponent<Joystick>();
            switch (user_type)
            {
                case 1:
                    socketTest_player1 = GameObject.Find("Player1Object").GetComponent<SocketTest_player1>();
                    socketTest_player1.enabled = false;
                    temp = GameObject.Find("Player1Object").GetComponent<PlayerControllerScript>();

                    temp.joystick = joy;
                    //temp.enabled = true;
                    if (MyJob)
                    {
                        vent = GameObject.Find("VentButton").GetComponent<VentButton>();
                        vent.player = GameObject.Find("Player1Object");
                        vent.enabled = true;
                        vent.isItStarted = true;

                        GameObject.Find("InteractButton").SetActive(false);

                        sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
                        sabotage.player = GameObject.Find("Player1Object");
                        sabotage.enabled = true;
                        sabotage.isItStarted = true;

                        GameObject temp_Impo_player1 = GameObject.Find("Player1Object");
                        kill.player = temp_Impo_player1;
                        GameObject temp_Impo_player2 = GameObject.Find("Player2Object");
                        GameObject temp_Impo_player3 = GameObject.Find("Player3Object");
                        GameObject temp_Impo_player4 = GameObject.Find("Player4Object");
                        GameObject temp_Impo_player5 = GameObject.Find("Player5Object");
                        kill.citizen[0] = temp_Impo_player2;
                        kill.citizen[1] = temp_Impo_player3;
                        kill.citizen[2] = temp_Impo_player4;
                        kill.citizen[3] = temp_Impo_player5;

                        kill.enabled = true;
                        kill.isItStarted = true;
                    }
                    else
                    {
                        interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                        interact.player = GameObject.Find("Player1Object");
                        interact.enabled = true;

                        GameObject.Find("VentButton").SetActive(false);
                        GameObject.Find("KillButton").SetActive(false);
                        //GameObject.Find("SabotageButton").SetActive(false);
                        GameObject.Find("SabotageButton").GetComponent<Button>().enabled = false;
                        GameObject.Find("SabotageButton").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                        GameObject.Find("SabotageButton").GetComponent<SabotageMap>().enabled = true;
                        GameObject.Find("SabotageButton").transform.GetChild(0).gameObject.SetActive(false);

                    }
                    temp_map = GameObject.Find("MapButton").GetComponent<MiniMap>();
                    GameObject temp_player = GameObject.Find("Player1Object");
                    temp_map.player = temp_player;
                    break;
                case 2:
                    socketTest_player2 = GameObject.Find("Player2Object").GetComponent<SocketTest_player2>();
                    socketTest_player2.enabled = false;
                    temp = GameObject.Find("Player2Object").GetComponent<PlayerControllerScript>();
                    temp.joystick = joy;
                    //temp.enabled = true;
                    if (MyJob)
                    {
                        vent = GameObject.Find("VentButton").GetComponent<VentButton>();
                        vent.player = GameObject.Find("Player2Object");
                        vent.enabled = true;
                        vent.isItStarted = true;

                        GameObject.Find("InteractButton").SetActive(false);

                        sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
                        sabotage.player = GameObject.Find("Player2Object");
                        sabotage.enabled = true;
                        sabotage.isItStarted = true;

                        GameObject temp_Impo_player2 = GameObject.Find("Player2Object");
                        kill.player = temp_Impo_player2;
                        GameObject temp_Impo_player1 = GameObject.Find("Player1Object");
                        GameObject temp_Impo_player3 = GameObject.Find("Player3Object");
                        GameObject temp_Impo_player4 = GameObject.Find("Player4Object");
                        GameObject temp_Impo_player5 = GameObject.Find("Player5Object");
                        kill.citizen[0] = temp_Impo_player1;
                        kill.citizen[1] = temp_Impo_player3;
                        kill.citizen[2] = temp_Impo_player4;
                        kill.citizen[3] = temp_Impo_player5;

                        kill.enabled = true;
                        kill.isItStarted = true;
                    }
                    else
                    {
                        interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                        interact.player = GameObject.Find("Player2Object");
                        interact.enabled = true;

                        GameObject.Find("VentButton").SetActive(false);
                        GameObject.Find("KillButton").SetActive(false);
                        //GameObject.Find("SabotageButton").SetActive(false);
                        GameObject.Find("SabotageButton").GetComponent<Button>().enabled = false;
                        GameObject.Find("SabotageButton").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                        GameObject.Find("SabotageButton").GetComponent<SabotageMap>().enabled = true;
                        GameObject.Find("SabotageButton").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    temp_map = GameObject.Find("MapButton").GetComponent<MiniMap>();
                    GameObject temp_player2 = GameObject.Find("Player2Object");
                    temp_map.player = temp_player2;
                    break;
                case 3:
                    socketTest_player3 = GameObject.Find("Player3Object").GetComponent<SocketTest_player3>();
                    socketTest_player3.enabled = false;
                    temp = GameObject.Find("Player3Object").GetComponent<PlayerControllerScript>();
                    temp.joystick = joy;
                    //temp.enabled = true;
                    if (MyJob)
                    {
                        vent = GameObject.Find("VentButton").GetComponent<VentButton>();
                        vent.player = GameObject.Find("Player3Object");
                        vent.enabled = true;
                        vent.isItStarted = true;

                        GameObject.Find("InteractButton").SetActive(false);

                        sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
                        sabotage.player = GameObject.Find("Player3Object");
                        sabotage.enabled = true;
                        sabotage.isItStarted = true;

                        GameObject temp_Impo_player3 = GameObject.Find("Player3Object");
                        kill.player = temp_Impo_player3;
                        GameObject temp_Impo_player1 = GameObject.Find("Player1Object");
                        GameObject temp_Impo_player2 = GameObject.Find("Player2Object");
                        GameObject temp_Impo_player4 = GameObject.Find("Player4Object");
                        GameObject temp_Impo_player5 = GameObject.Find("Player5Object");
                        kill.citizen[0] = temp_Impo_player1;
                        kill.citizen[1] = temp_Impo_player2;
                        kill.citizen[2] = temp_Impo_player4;
                        kill.citizen[3] = temp_Impo_player5;

                        kill.enabled = true;
                        kill.isItStarted = true;
                    }
                    else
                    {
                        interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                        interact.player = GameObject.Find("Player3Object");
                        interact.enabled = true;

                        GameObject.Find("VentButton").SetActive(false);
                        GameObject.Find("KillButton").SetActive(false);
                        //GameObject.Find("SabotageButton").SetActive(false);
                        GameObject.Find("SabotageButton").GetComponent<Button>().enabled = false;
                        GameObject.Find("SabotageButton").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                        GameObject.Find("SabotageButton").GetComponent<SabotageMap>().enabled = true;
                        GameObject.Find("SabotageButton").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    temp_map = GameObject.Find("MapButton").GetComponent<MiniMap>();
                    GameObject temp_player3 = GameObject.Find("Player3Object");
                    temp_map.player = temp_player3;
                    break;
                case 4:
                    socketTest_player4 = GameObject.Find("Player4Object").GetComponent<SocketTest_player4>();
                    socketTest_player4.enabled = false;
                    temp = GameObject.Find("Player4Object").GetComponent<PlayerControllerScript>();
                    temp.joystick = joy;
                    //temp.enabled = true;
                    if (MyJob)
                    {
                        vent = GameObject.Find("VentButton").GetComponent<VentButton>();
                        vent.player = GameObject.Find("Player4Object");
                        vent.enabled = true;
                        vent.isItStarted = true;

                        GameObject.Find("InteractButton").SetActive(false);

                        sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
                        sabotage.player = GameObject.Find("Player4Object");
                        sabotage.enabled = true;
                        sabotage.isItStarted = true;

                        GameObject temp_Impo_player4 = GameObject.Find("Player4Object");
                        kill.player = temp_Impo_player4;
                        GameObject temp_Impo_player1 = GameObject.Find("Player1Object");
                        GameObject temp_Impo_player2 = GameObject.Find("Player2Object");
                        GameObject temp_Impo_player3 = GameObject.Find("Player3Object");
                        GameObject temp_Impo_player5 = GameObject.Find("Player5Object");
                        kill.citizen[0] = temp_Impo_player1;
                        kill.citizen[1] = temp_Impo_player2;
                        kill.citizen[2] = temp_Impo_player3;
                        kill.citizen[3] = temp_Impo_player5;

                        kill.enabled = true;
                        kill.isItStarted = true;
                    }
                    else
                    {
                        interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                        interact.player = GameObject.Find("Player4Object");
                        interact.enabled = true;

                        GameObject.Find("VentButton").SetActive(false);
                        GameObject.Find("KillButton").SetActive(false);
                        //GameObject.Find("SabotageButton").SetActive(false);
                        GameObject.Find("SabotageButton").GetComponent<Button>().enabled = false;
                        GameObject.Find("SabotageButton").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                        GameObject.Find("SabotageButton").GetComponent<SabotageMap>().enabled = true;
                        GameObject.Find("SabotageButton").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    temp_map = GameObject.Find("MapButton").GetComponent<MiniMap>();
                    GameObject temp_player4 = GameObject.Find("Player4Object");
                    temp_map.player = temp_player4;
                    break;
                case 5:
                    socketTest_player5 = GameObject.Find("Player5Object").GetComponent<SocketTest_player5>();
                    socketTest_player5.enabled = false;
                    temp = GameObject.Find("Player5Object").GetComponent<PlayerControllerScript>();
                    temp.joystick = joy;
                    //temp.enabled = true;
                    if (MyJob)
                    {
                        vent = GameObject.Find("VentButton").GetComponent<VentButton>();
                        vent.player = GameObject.Find("Player5Object");
                        vent.enabled = true;
                        vent.isItStarted = true;

                        GameObject.Find("InteractButton").SetActive(false);

                        sabotage.player = GameObject.Find("Player5Object");
                        sabotage.enabled = true;
                        sabotage.isItStarted = true;

                        GameObject temp_Impo_player5 = GameObject.Find("Player5Object");
                        kill.player = temp_Impo_player5;
                        GameObject temp_Impo_player1 = GameObject.Find("Player1Object");
                        GameObject temp_Impo_player2 = GameObject.Find("Player2Object");
                        GameObject temp_Impo_player3 = GameObject.Find("Player3Object");
                        GameObject temp_Impo_player4 = GameObject.Find("Player4Object");
                        kill.citizen[0] = temp_Impo_player1;
                        kill.citizen[1] = temp_Impo_player2;
                        kill.citizen[2] = temp_Impo_player3;
                        kill.citizen[3] = temp_Impo_player4;

                        kill.enabled = true;
                        kill.isItStarted = true;
                    }
                    else
                    {
                        interact = GameObject.Find("InteractButton").GetComponent<InteractButton>();
                        interact.player = GameObject.Find("Player5Object");
                        interact.enabled = true;

                        GameObject.Find("VentButton").SetActive(false);
                        GameObject.Find("KillButton").SetActive(false);
                        //GameObject.Find("SabotageButton").SetActive(false);
                        GameObject.Find("SabotageButton").GetComponent<Button>().enabled = false;
                        GameObject.Find("SabotageButton").GetComponent<Image>().color = new Color(255, 255, 255, 0);
                        GameObject.Find("SabotageButton").GetComponent<SabotageMap>().enabled = true;
                        GameObject.Find("SabotageButton").transform.GetChild(0).gameObject.SetActive(false);
                    }
                    temp_map = GameObject.Find("MapButton").GetComponent<MiniMap>();
                    GameObject temp_player5 = GameObject.Find("Player5Object");
                    temp_map.player = temp_player5;
                    break;
            }
            user_type_check = false;
        }
    }

    IEnumerator OpenCloseDoor(int index, int time)
    {
        sabotage = GameObject.Find("SabotageButton").GetComponent<SabotageMap>();
        sabotage.Close_The_Door(index, true);
        while (true) //무한 반복
        {
            //종료
            if (time == 0)
            {
                sabotage.Close_The_Door(index, false);
                break;
            }
            //투표 진행중
            else
            {
                time--; //1씩 감소
                yield return new WaitForSeconds(1f); //1초 딜레이
            }
        }
    }
}

public class SabotageIndeX
{
    public int index;
}

public class GetPlayerMeeting
{
    public bool isItMeeting;
    public string playerName;
}