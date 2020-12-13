using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basketball : MonoBehaviour
{
    //농구장 미니게임 패널 게임오브젝트
    private GameObject basketball;
    //농구공 날라가는 스크립트
    private Ball_Move ball;
    //게이지 용량(아래/중간/위) 이미지 배열 생성
    private const int numOfMeters = 3;
    private Image[] meterImg = new Image[numOfMeters];
    //공의 초기 위치값
    private Vector3 ballPosition;
    //공의 도착되는 위치 객체
    private Transform[] net = new Transform[numOfMeters];
    //게이지의 진행에 따른 string 변수
    private string meterState;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject basketball_hoop;

    // Start is called before the first frame update
    void Start()
    {
        basketball = this.transform.parent.gameObject;
        ball = this.transform.GetChild(0).gameObject.GetComponent<Ball_Move>();
        ballPosition = this.transform.GetChild(0).gameObject.transform.localPosition;
        for (int i = 0; i < numOfMeters; i++)
        {
            meterImg[i] = this.transform.GetChild(5).transform.GetChild(0).transform.GetChild(numOfMeters - (i + 1)).GetComponent<Image>();
            meterImg[i].fillAmount = 0;
            net[i] = this.transform.GetChild(i+1).gameObject.transform.GetComponent<Transform>();
        }
        mini_game = basketball.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
        meterState = "Going Up";
    }

    // Update is called once per frame
    void Update()
    {
        switch (meterState)
        {
            case "Going Up":
                increase_overall_gauge(meterImg);
                break;
            case "Going Down":
                decrease_overall_gauge(meterImg);
                break;
            case "Went Up":
                Shooting_Power();
                break;
            case "Went Down":
                Shooting_Power();
                break;
        }
    }

    public void increase_overall_gauge(Image[] gauges)
    {
        for (int i = 0; i < numOfMeters; i++)
        {
            if (gauges[i].fillAmount < 1)
            {
                gauges[i].fillAmount += 0.025f;
                break;
            }
            else if (gauges[0].fillAmount == 1 && gauges[1].fillAmount == 1 && gauges[2].fillAmount == 1)
            {
                meterState = "Going Down";
            }
        }
    }

    public void decrease_overall_gauge(Image[] gauges)
    {
        for (int i = numOfMeters - 1; i >= 0; i--)
        {
            if (gauges[i].fillAmount > 0)
            {
                gauges[i].fillAmount -= 0.025f;
                break;
            }
            else if (gauges[0].fillAmount == 0 && gauges[1].fillAmount == 0 && gauges[2].fillAmount == 0)
            {
                meterState = "Going Up";
            }
        }
    }

    public void shoot_the_ball()
    {
        switch (meterState)
        {
            case "Going Up":
                meterState = "Went Up";
                break;
            case "Going Down":
                meterState = "Went Down";
                break;
            case "Went Up":
                meterState = "Going Up";
                break;
            case "Went Down":
                meterState = "Going Down";
                break;
        }
    }

    public void Shooting_Power()
    {
        if (meterImg[0].fillAmount == 1 && meterImg[1].fillAmount == 1 && meterImg[2].fillAmount > 0)
        {
            if (ball.Shoot_the_Ball(net[0]))
            {
                Init_Ball_And_Gauge();
            }
        }
        else if (meterImg[0].fillAmount == 1 && meterImg[1].fillAmount > 0 && meterImg[2].fillAmount == 0)
        {
            if (ball.Shoot_the_Ball(net[1]))
            {
                Exit_Panel();
            }
        }
        else if (meterImg[0].fillAmount > 0 && meterImg[1].fillAmount == 0 && meterImg[2].fillAmount == 0)
        {
            if (ball.Shoot_the_Ball(net[2]))
            {
                Init_Ball_And_Gauge();
            }
        }
    }

    private void Init_Ball_And_Gauge()
    {
        ball.transform.localPosition = ballPosition;
        if(meterState == "Went Up")
        {
            meterState = "Going Up";
        }
        else if (meterState == "Went Down")
        {
            meterState = "Going Down";
        }
    }

    private void Exit_Panel()
    {
        basketball.SetActive(false);
        mini_game.ClearMiniGame(basketball_hoop);
        if (basketball_hoop.min < basketball_hoop.max)
        {
            ball.transform.localPosition = ballPosition;
            for (int i = 0; i < numOfMeters; i++)
            {
                meterImg[i].fillAmount = 0;
            }
            meterState = "Going Up";
        }
    }
}