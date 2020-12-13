using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Discuss : MonoBehaviour
{
    //토론 패널
    private GameObject discuss;
    private Discuss discuss_script;
    public GameObject voteResult;

    //토론 패널 메인 텍스트
    private Text mainText;
    private string beforeVoting;
    private string afterVoting;

    //메시지 알람 이미지
    private GameObject alertMessage;
    public bool isItAlert;
    public bool justOne;
    private bool isItGet = false;

    //플레이어 수
    private const int numOfPlayer = 5;

    public GameObject player;
    //나의 인덱스
    public int myIndex;
    //투표한 사람의 인덱스(서버)
    public int whoIsVoted = numOfPlayer;
    //투표 받은 사람의 인덱스(서버)
    private int whoIsPointedOut;

    //플레이어 버튼
    private Button[] playerButton = new Button[numOfPlayer];

    //플레이어 이미지
    private Image[] playerImage = new Image[numOfPlayer];
    public string[] playerImageString = new string[numOfPlayer];
    private Sprite[] playerImageSprite = new Sprite[numOfPlayer];

    //플레이어 이름
    private Text[] playerText = new Text[numOfPlayer];
    public string[] playerTextString = new string[numOfPlayer];

    //투표한 플레이어 이미지
    private Image[,] votedPlayerImage = new Image[numOfPlayer, numOfPlayer];

    //신고자를 구별해주는 이미지
    private Image[] bellImage = new Image[numOfPlayer];

    public string playerMeetingId;

    //해당 플레이어를 투표하는 버튼
    private GameObject[] voteButton = new GameObject[numOfPlayer];
    private Button[] voteBtn = new Button[numOfPlayer];

    //해당 플레이어를 투표하지 않는 버튼
    private GameObject[] notVoteButton = new GameObject[numOfPlayer];

    //투표했는지 확인하는 이미지
    public Image[] votedImage = new Image[numOfPlayer];
    private bool[] isItVoted = new bool[numOfPlayer];

    //투표 스킵 버튼
    private GameObject voteSkipButton;
    //투표를 스킵하는 버튼
    private GameObject skipButton;
    private Button skipBtn;
    //투표를 스킵하지 않는 버튼
    private GameObject notSkipButton;

    //투표 결과 중 투표 스킵 텍스트
    private GameObject skippedVotingText;
    //투표 스킵한 플레이어 이미지
    private Image[] skippedVotingPlayerImage = new Image[numOfPlayer];

    //투표 타이머
    private Text voteTimer;
    private string byTheEndOfTheVote;
    private string upToConfirmation;
    private bool isDone;
    private bool vote_done = false;
    private string skipVoteText;
    private IEnumerator coroutine;

    private VotePlayer sendVotePlayer;
    public ChatManager chat;
    //채팅창
    private GameObject chatWindow;
	private GameObject Joystick;

    //투명도 설정 컬러객체
    private Color normal = new Color(255, 255, 255, 255);
    private Color half = new Color(255, 255, 255, 128);
    private Color transparent = new Color(255, 255, 255, 0);

    public void Init()
    {
        //토론 패널
        discuss = this.transform.parent.gameObject;
        discuss_script = discuss.GetComponent<Discuss>();

        //토론 패널 메인 텍스트
        mainText = this.transform.GetChild(0).GetComponent<Text>();
        beforeVoting = "임포스터는 누구인가?";
        afterVoting = "투표 결과";
        mainText.text = beforeVoting;

        //메시지 알람 이미지
        alertMessage = this.transform.GetChild(1).transform.GetChild(0).gameObject;
        isItAlert = false;
        justOne = true;
        alertMessage.SetActive(isItAlert);

        //투표 결과 중 투표 스킵 텍스트
        skippedVotingText = this.transform.GetChild(8).gameObject;
        skippedVotingText.SetActive(false);

        for (int i = 0; i < numOfPlayer; i++)
        {
            //플레이어 버튼
            playerButton[i] = this.transform.GetChild(i + 2).GetComponent<Button>();
            playerButton[i].interactable = true;


            //플레이어 이미지
            playerImage[i] = this.transform.GetChild(i + 2).transform.GetChild(0).GetComponent<Image>();
            playerImageSprite[i] = Resources.Load<Sprite>(playerImageString[i]);
            playerImage[i].sprite = playerImageSprite[i];

            //플레이어 이름
            playerText[i] = this.transform.GetChild(i + 2).transform.GetChild(1).GetComponent<Text>();
            playerText[i].text = playerTextString[i];

            //투표한 플레이어 이미지
            for (int j = 0; j < numOfPlayer; j++)
            {
                votedPlayerImage[i, j] = this.transform.GetChild(i + 2).transform.GetChild(2).transform.GetChild(j).GetComponent<Image>();
                votedPlayerImage[i, j].sprite = null;
                votedPlayerImage[i, j].color = transparent;
            }

            //신고자를 구별해주는 이미지
            bellImage[i] = this.transform.GetChild(i + 2).transform.GetChild(3).GetComponent<Image>();
            if (playerTextString[i] != playerMeetingId)
            {
                bellImage[i].color = transparent;
            }

            //해당 플레이어를 투표하는 버튼
            voteButton[i] = this.transform.GetChild(i + 2).transform.GetChild(4).gameObject;
            voteButton[i].SetActive(false);

            //해당 플레이어를 투표하지 않는 버튼
            notVoteButton[i] = this.transform.GetChild(i + 2).transform.GetChild(5).gameObject;
            notVoteButton[i].SetActive(false);

            //투표했는지 확인하는 이미지
            votedImage[i] = this.transform.GetChild(i + 2).transform.GetChild(6).GetComponent<Image>();
            votedImage[i].color = transparent;

            //투표 스킵한 플레이어 이미지
            skippedVotingPlayerImage[i] = skippedVotingText.transform.GetChild(0).transform.GetChild(i).GetComponent<Image>();
            skippedVotingPlayerImage[i].sprite = null;
            skippedVotingPlayerImage[i].color = transparent;
        }

        //투표 스킵 버튼
        voteSkipButton = this.transform.GetChild(7).gameObject;
        voteSkipButton.SetActive(true);

        //투표를 스킵하는 버튼
        skipButton = voteSkipButton.transform.GetChild(1).gameObject;
        skipButton.SetActive(false);

        //투표를 스킵하지 않는 버튼
        notSkipButton = voteSkipButton.transform.GetChild(2).gameObject;
        notSkipButton.SetActive(false);

        //투표 타이머
        voteTimer = this.transform.GetChild(9).GetComponent<Text>();
        voteTimer.text = null;
        byTheEndOfTheVote = "투표 종료까지 : ";
        upToConfirmation = "확인까지 : ";
        isDone = false;
        coroutine = Voting_Timer(120);

        //채팅창
        chatWindow = this.transform.GetChild(10).gameObject;
        chatWindow.SetActive(false);

        SocketManger.Socket.On("GetPlayerVote", (data) =>
        {
            //서버에서 받은 데이터값
            VotePlayer getVotePlayer = new VotePlayer();
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            getVotePlayer = JsonUtility.FromJson<VotePlayer>(json);

            whoIsVoted = getVotePlayer.votedPlayerIndex;
            whoIsPointedOut = getVotePlayer.pointedOutPlayerIndex;
            Debug.Log("서버에서 전달 받음");
            isItGet = true;
        });

        StartCoroutine(coroutine);
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (player.name)
        {
            case "Player1Object":
                myIndex = 0;
                break;
            case "Player2Object":
                myIndex = 1;
                break;
            case "Player3Object":
                myIndex = 2;
                break;
            case "Player4Object":
                myIndex = 3;
                break;
            case "Player5Object":
                myIndex = 4;
                break;
        }
        Init();
    }

    void Update()
    {
        if (isItGet)
        {
            Vote_The_Player(whoIsVoted, whoIsPointedOut);
            isItGet = false;
        }

        if (isItAlert && justOne)
        {
            alertMessage.SetActive(isItAlert);
            justOne = false;
        }

        for (int i = 0; i < numOfPlayer; i++)
        {
            if(isItVoted[0]&& isItVoted[1] && isItVoted[2] && isItVoted[3] && isItVoted[4])
            {
                vote_done = true;
            }
            if (votedImage[i].color == normal)
            {
                isItVoted[i] = true;
            }
        }
    }

    public void Show_Vote_Button(int index)
    {
        if (index == numOfPlayer)
        {
            for (int i = 0; i < numOfPlayer; i++)
            {
                if (voteButton[i].activeSelf == true && notVoteButton[i].activeSelf == true)
                {
                    voteButton[i].SetActive(false);
                    notVoteButton[i].SetActive(false);
                }
            }
            if (skipButton.activeSelf == false && notSkipButton.activeSelf == false)
            {
                skipButton.SetActive(true);
                notSkipButton.SetActive(true);

                skipBtn = skipButton.GetComponent<Button>();
                skipBtn.onClick.AddListener(() => Vote_The_Player(myIndex, numOfPlayer));
            }
        }
        else
        {
            skipButton.SetActive(false);
            notSkipButton.SetActive(false);
            for (int i = 0; i < numOfPlayer; i++)
            {
                if (index != i && voteButton[i].activeSelf == true && notVoteButton[i].activeSelf == true)
                {
                    voteButton[i].SetActive(false);
                    notVoteButton[i].SetActive(false);
                }
                else if (index == i && voteButton[index].activeSelf == false && notVoteButton[index].activeSelf == false)
                {
                    voteButton[index].SetActive(true);
                    notVoteButton[index].SetActive(true);

                    voteBtn[index] = voteButton[index].GetComponent<Button>();
                    voteBtn[index].onClick.AddListener(() => Vote_The_Player(myIndex, index));
                }
            }
        }
    }

    public void Cancel_The_Vote(int index)
    {
        if (index == numOfPlayer)
        {
            skipButton.SetActive(false);
            notSkipButton.SetActive(false);
        }
        else
        {
            voteButton[index].SetActive(false);
            notVoteButton[index].SetActive(false);
        }
    }

    public void Vote_The_Player(int myindex, int otherindex)
    {
        if (myIndex == myindex)
        {
            sendVotePlayer = new VotePlayer();
            sendVotePlayer.votedPlayerIndex = myindex;//4
            sendVotePlayer.pointedOutPlayerIndex = otherindex;//0

            string data = JsonUtility.ToJson(sendVotePlayer, prettyPrint: true);
            SocketManger.Socket.Emit("SendPlayerVote", data);
            Debug.Log("내가 보낸 데이터 서버 전송" + myindex);

            Cancel_The_Vote(otherindex);
            voteSkipButton.SetActive(false);
            for (int i = 0; i < numOfPlayer; i++)
            {
                playerButton[i].interactable = false;
            }
        }
        Debug.Log("상대방이 보낸 데이터 서버 전송" + myindex);
        //해당 플레이어의 케릭터에 표시해야함
        votedImage[myindex].color = normal;
        isItAlert = true;
        justOne = true;

        if (otherindex == numOfPlayer)
        {
            skippedVotingPlayerImage[myindex].sprite = playerImageSprite[myindex];
        }
        else
        {
            votedPlayerImage[otherindex, myindex].sprite = playerImageSprite[myindex];
        }
    }

    public void Open_Chat_Window()
    {
        if (chatWindow.activeSelf == false)
        {
            chatWindow.SetActive(true);
            if (alertMessage.activeSelf == true)
            {
                isItAlert = false;
                justOne = true;
            }
        }
        else
        {
            chatWindow.SetActive(false);
        }
    }

    IEnumerator Voting_Timer(int time)
    {
        while (true) //무한 반복
        {
            voteTimer.text = byTheEndOfTheVote + time + "s"; //1000단위
                                                             //종료
            if (time == 0 && isDone == true)
            {
                voteTimer.text = null;
                //엔딩패널 키고
                voteResult.SetActive(true);
                discuss.SetActive(false);
                Init();
                chat.Init();
                break;
            }
            //투표 결과 확인중
            else if ((time == 0 && isDone == false) || vote_done == true)
            {
                mainText.text = afterVoting;
                skippedVotingText.SetActive(true);

                //투표결과 공개(플레이어 간 투표)
                for (int i = 0; i < numOfPlayer; i++)
                {
                    for (int j = 0; j < numOfPlayer; j++)
                    {
                        if (votedPlayerImage[i, j].sprite != null)
                        {
                            votedPlayerImage[i, j].color = normal;
                        }
                    }
                }

                //투표결과 공개(스킵된 투표)
                for (int i = 0; i < numOfPlayer; i++)
                {
                    if (skippedVotingPlayerImage[i].sprite != null)
                    {
                        skippedVotingPlayerImage[i].color = normal;
                    }
                }

                time = 5;
                while (true) //무한 반복
                {
                    voteTimer.text = "<color=#ff0000>" + upToConfirmation + time + "s</color>"; //1000단위
                    if (time == 0)
                    {
                        isDone = true;
                        break;
                    }
                    else
                    {
                        time--; //1씩 감소
                        yield return new WaitForSeconds(1f); //1초 딜레이
                    }
                }
            }
            //투표 진행중
            else if (time != 0 && isDone == false)
            {
                time--; //1씩 감소
                yield return new WaitForSeconds(1f); //1초 딜레이
            }
        }
    }
}

public class VotePlayer
{
    //투표를 한사람 인덱스
    public int votedPlayerIndex;
    //투표를 받은사람 인덱스
    public int pointedOutPlayerIndex;
}