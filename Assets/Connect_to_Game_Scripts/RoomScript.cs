using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{
    //PlayMenu의 Roomname, Usercount Text지정.
    public Text RoomName_Play, UserCount1, UserCount2;
    public InputField Name_text, ID_text;//Nickname, userID InputField 지정.
    //JoinMenu의 Roomname, Startcount Text지정.
    public Text RoomName_Join, StartCount;
    //user현황 Image, Text를 배열로 지정.
    public Image[] UserCharacter = new Image[5];
    public Text[] UserName = new Text[5];
    public Image[] UserPannel = new Image[5];
    public static string[] input_userName = new string[5];
    public static int[] input_userCharacter = new int[5];
    public static bool[] UserReadycount = new bool[5];

    [SerializeField]
    private Sprite[] sprites;

    public static int ReadyCount = 0, length = 0, length2 = 0, imagenumber;
    public static string usercount1="", usercount2="";
    public static bool isPlayBtnClick = false, GameStart = false;

    void Start()
    {
        SocketManger.Socket.On("resWaitingRoom", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            RoomUserCount response_room = JsonUtility.FromJson<RoomUserCount>(json);
            usercount1 = response_room.room1 + "/5";
            usercount2 = response_room.room2 + "/5";
            isPlayBtnClick = true;
        });
        SocketManger.Socket.On("JoinRoomUserInfo", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            UserInfo response_user = JsonUtility.FromJson<UserInfo>(json);
            length = response_user.USER.Length;
            length2 = response_user.USER.Length;
            for (int i = 0; i < response_user.USER.Length; i++)
            {
                input_userCharacter[i] = response_user.USER[i].UserCharacter;
                input_userName[i] = response_user.USER[i].UserName;
                ReadyCount = response_user.GameStartCount;
                UserReadycount[i] = response_user.USER[i].readyCheck;
            }
        });
        SocketManger.Socket.On("GameStartCount", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            UserInfo g_response_user = JsonUtility.FromJson<UserInfo>(json);
            length = g_response_user.USER.Length;
            length2 = g_response_user.USER.Length;
            ReadyCount = g_response_user.GameStartCount;
            for(int i=0; i<g_response_user.USER.Length; i++) UserReadycount[i] = g_response_user.USER[i].readyCheck;
        });
        SocketManger.Socket.On("GameStart", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            SceneDeliver.sceneDeliver.playerInfo = JsonUtility.FromJson<UserInfo>(json);
            SceneDeliver.sceneDeliver.userID = ID_text.text;
            GameStart = true;
        });
        SocketManger.Socket.On("NewLeaveRoomUserInfo", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            UserInfo leave_user = JsonUtility.FromJson<UserInfo>(json);
            length = leave_user.USER.Length;
            length2 = leave_user.USER.Length;
            for (int i = 0; i < leave_user.USER.Length; i++)
            {
                input_userCharacter[i] = leave_user.USER[i].UserCharacter;
                input_userName[i] = leave_user.USER[i].UserName;
                ReadyCount = leave_user.GameStartCount;
                UserReadycount[i] = leave_user.USER[i].readyCheck;
            }
        });
    }
    public void PlayBtncclick()
    {
        string data = "";
        SocketManger.Socket.Emit("Waiting", data);
    }
    public void JoinBtnclick()
    {
        //서버에 내 유저정보 전송
        RoomPushData roomPushData = new RoomPushData();
        CharacterType c_user_Character = GameObject.Find("SelectBtn").GetComponent<CharacterType>();
        roomPushData.RoomName = RoomName_Play.GetComponent<Text>().text;
        roomPushData.UserID = ID_text.text;
        roomPushData.UserName = Name_text.text;
        roomPushData.Character = c_user_Character.c_character_number;
        RoomName_Join.GetComponent<Text>().text = roomPushData.RoomName;
        string data = JsonUtility.ToJson(roomPushData, prettyPrint: true);
        SocketManger.Socket.Emit("JoinRoom", data);
    }
    public void StartBtnclick()
    {
        UserReadyCheck userreadycheck = new UserReadyCheck();
        if(userreadycheck.ReadyCheck)
        {
            userreadycheck.ReadyCheck = false;
            userreadycheck.RoomName = RoomName_Play.GetComponent<Text>().text;
            userreadycheck.UserID = ID_text.text;
            string data = JsonUtility.ToJson(userreadycheck, prettyPrint: true);
            SocketManger.Socket.Emit("ReadyButton", data);
        }
        else
        {
            userreadycheck.ReadyCheck = true;
            userreadycheck.RoomName = RoomName_Join.GetComponent<Text>().text;
            userreadycheck.UserID = ID_text.text;
            string data = JsonUtility.ToJson(userreadycheck, prettyPrint: true);
            SocketManger.Socket.Emit("ReadyButton", data);
            Toast.Instance.Show("Ready. 5명 모두 준비가 될 경우 게임을 시작합니다.", 1f);
        }
    }
    public void BackBtnclick()
    {
        RoomPushData roomPushData = new RoomPushData();
        roomPushData.UserID = ID_text.text;
        roomPushData.RoomName = RoomName_Join.GetComponent<Text>().text;
        string data = JsonUtility.ToJson(roomPushData, prettyPrint: true);
        SocketManger.Socket.Emit("LeaveRoom", data);
    }
    private void FixedUpdate()
    {
        if(isPlayBtnClick)
        {
            UserCount1.GetComponent<Text>().text = usercount1;
            UserCount2.GetComponent<Text>().text = usercount2;
            isPlayBtnClick = false;
        }
        if (length > 0)
        {
            for(int i=0; i<5;i++)
            {
                UserCharacter[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
                UserName[i].GetComponent<Text>().color = new Color(0, 0, 0, 0);
            }
            for (int i = 0; i < length; i++)
            {
                UserCharacter[i].sprite = sprites[input_userCharacter[i]];
                UserCharacter[i].color = new Color(255, 255, 255, 255);
                UserName[i].text = input_userName[i];
                UserName[i].color = new Color(0, 0, 0, 255);
            }
            length = 0;
        }
        if(length2 >0)
        {
            for(int i=0; i<5;i++) UserPannel[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            for (int i = 0; i<length2; i++)
            {
                if (UserReadycount[i]) UserPannel[i].GetComponent<Image>().color = new Color(0, 255, 0, 255);
                else UserPannel[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }
            StartCount.GetComponent<Text>().text = "Ready\n" + ReadyCount + "/5";
            length2 = 0;
        }
        if(GameStart)
        {
            GameStart = false;
            SceneManager.LoadScene("LoadingScene");
        }
    }
}
public class RoomUserCount {
    public int room1, room2;
}
public class RoomPushData
{
    public string RoomName, UserID, UserName;
    public int Character;
}
[System.Serializable]
public class AddThisPlayer
{
    public string UserID, UserName;
    public int joinRoomNumber, imageNumber, UserCharacter;
    public bool readyCheck;
    public bool isImposter;
}
[System.Serializable]
public class UserInfo
{
    public AddThisPlayer[] USER;
    public int GameStartCount;
}
public class UserReadyCheck
{
    public bool ReadyCheck = false;
    public string UserID;
    public string RoomName;
}
