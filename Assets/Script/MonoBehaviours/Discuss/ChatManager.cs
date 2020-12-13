using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ChatManager : MonoBehaviour
{
    //채팅창 내 인원수
    public const int numOfPlayer = 5;
    //내가 보내는 메시지
    public GameObject myKakaoTalk;
    //타인이 보내는 메시지
    public GameObject otherKakaoTalk;
    //투표 정보를 보내는 메시지
    public GameObject informationKakaoTalk;
    //채팅창 가로길이
    private RectTransform ContentRect;
    //채팅창 스크롤바
    private Scrollbar scrollBar;
    //채팅 입력
    private InputField inputField;
    //메시지 길이
    private Text lengthOfText;
    //토론 스크립트
    public Discuss discuss;
    //투표 정보 제공 메시지
    private int numOfVoted;
    private bool[] isItCreated = new bool[numOfPlayer];
    //서버로 전송할때의 클래스이름
    private Message message;
    private int myIndex;
    private int whoIsVoted;
    private int prevVoted;
    //서버에서 전달받았을떄의 클래스이름
    private Message response;
    private string otherImagePath;
    private string otherName;
    private string otherText;
    private bool isItGet = false;

    public void Init()
    {
        ContentRect = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();

        scrollBar = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<Scrollbar>();

        inputField = this.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<InputField>();

        lengthOfText = this.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        lengthOfText.text = "(0/50)";

        numOfVoted = 5;
        for (int i = 0; i < numOfVoted; i++)
        {
            isItCreated[i] = false;
        }

        SocketManger.Socket.On("GetMessage", (data) => {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            response = JsonUtility.FromJson<Message>(json);
            otherImagePath = response.imagePath;
            otherName = response.name;
            otherText = response.text;
            isItGet = true;
            discuss.isItAlert = true;
            discuss.justOne = true;
        });
    }

    void Start()
    {
        myIndex = discuss.myIndex;
        whoIsVoted = numOfPlayer;
        prevVoted = whoIsVoted;

        Init();
    }
	void OnEnable(){
		Init();
	}

    void Update()
    {
        if(inputField.text == null)
        {
            lengthOfText.text = "(0/50)";
        }
        else
        {
            lengthOfText.text = "("+ inputField.text.Length + "/50)";
        }

        if(isItGet)
        {
            Chat(false, otherText, otherName, Resources.Load<Sprite>(otherImagePath));
            isItGet = false;
        }

        whoIsVoted = discuss.whoIsVoted;
        if (whoIsVoted != numOfPlayer && whoIsVoted != prevVoted)
        {
            numOfVoted--;
            // 이전 것과 날짜가 다르면 날짜영역 보이기
            Transform CurDateArea = Instantiate(informationKakaoTalk).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex());
            CurDateArea.GetComponent<KakaoTalk>().informationText.text = discuss.playerTextString[whoIsVoted] + " 이(가) 투표했습니다. " + numOfVoted.ToString() + " 명 남음.";
            CurDateArea.GetComponent<KakaoTalk>().userImage.sprite = Resources.Load<Sprite>(discuss.playerImageString[whoIsVoted]);
            CurDateArea.GetComponent<KakaoTalk>().votedImage.SetActive(true);
            discuss.isItAlert = true;
            discuss.justOne = true;

            prevVoted = whoIsVoted;
        }
    }

    public void Chat(bool isSend, string text, string user, Sprite picture) 
    {
        if (text.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;

        //보내는 사람은 노랑, 받는 사람은 흰색영역을 크게 만들고 텍스트 대입
        KakaoTalk kakaoTalk = Instantiate(isSend ? myKakaoTalk : otherKakaoTalk).GetComponent<KakaoTalk>();
        kakaoTalk.transform.SetParent(ContentRect.transform, false);
        kakaoTalk.messageBoxRect.sizeDelta = new Vector2(600, kakaoTalk.messageBoxRect.sizeDelta.y);
        kakaoTalk.messageTextRect.GetComponent<Text>().text = text;
        Fit(kakaoTalk.messageBoxRect);

        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
        float X = kakaoTalk.messageTextRect.sizeDelta.x + 42;
        float Y = kakaoTalk.messageTextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                kakaoTalk.messageBoxRect.sizeDelta = new Vector2(X - i * 2, kakaoTalk.messageBoxRect.sizeDelta.y);
                Fit(kakaoTalk.messageBoxRect);

                if (Y != kakaoTalk.messageTextRect.sizeDelta.y) { kakaoTalk.messageBoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else kakaoTalk.messageBoxRect.sizeDelta = new Vector2(X, Y);

        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        kakaoTalk.Time = t.ToString("yyyy-MM-dd-HH-mm");
        kakaoTalk.userNameText.text = user;
        kakaoTalk.userImage.sprite = picture;

        if(isItCreated[0] == true && isSend == true)
        {
            kakaoTalk.votedImage.SetActive(true);
        }

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        kakaoTalk.timeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");

        Fit(kakaoTalk.messageBoxRect);
        Fit(kakaoTalk.katalkRect);
        Fit(ContentRect);

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay() => scrollBar.value = 0;

    public void Send_Message()
    {
        string text = inputField.text;
        message = new Message();
        message.imagePath = discuss.playerImageString[myIndex];
        message.name = discuss.playerTextString[myIndex];
        message.text = text;
        string msg = JsonUtility.ToJson(message, prettyPrint: true);
        SocketManger.Socket.Emit("SendMessage", msg);
        Chat(true, text, message.name, Resources.Load<Sprite>(message.imagePath));
        inputField.text = null;
        discuss.isItAlert = true;
        discuss.justOne = true;
    }
}

public class Message
{
    //메세지 보내는 사람의 사진주소
    public string imagePath;
    //메세지 보내는 사람의 이름
    public string name;
    //메세지 내용
    public string text;
}