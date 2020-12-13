using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KakaoTalk : MonoBehaviour
{
    //KaTalk 전체적인 크기 변수
    public RectTransform katalkRect;
    //Message Box 전체적인 크기 변수
    public RectTransform messageBoxRect;
    //Message Text 크기 변수
    public RectTransform messageTextRect;
    //public GameObject Tail;
    //메시지 로그 시간 텍스트
    public Text timeText;
    //작성자 이름 텍스트
    public Text userNameText;
    //투표 정보 텍스트
    public Text informationText;
    //작성자 사진 이미지
    public Image userImage;
    //투표했는지 보여주는 이미지
    public GameObject votedImage;
    //메시지 로그 시간 문자열
    public string Time;
    //작성자 이름 문자열
    public string userName;
    //투표 정보 프리팹은 한번만 생성되어야하기 떄문에 bool로 이전에 생성 여부 판단
    public bool isItCreated;
}
