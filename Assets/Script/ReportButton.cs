using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportButton : MonoBehaviour
{
    //플레이어 게임오브젝트
    public GameObject player;
    //플레이어 근처 무덤 오브젝트
    public GameObject nearbyObject;
    //리포트 버튼
    private Button reportButton;
    //리포트 버튼 이미지
    private Image reportButtonImage;
    //킬 버튼 스크립트(무덤 인스턴스 리스트 사용)
    public KillButton killBtn;

    //컬러 객체(리포트 버튼 활성화 유무)
    private Color normal = new Color(255, 255, 255, 255);
    private Color transparent = new Color(255, 255, 255, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        reportButton = this.GetComponent<Button>();
        reportButtonImage = this.GetComponent<Image>();
        ChangeButtonUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어의 위치에 따른 미션오브젝트 리스트 내역 거리 비교 및 UI변경
        Vector3 posPlayer = player.transform.position;
        FindGraveStone(posPlayer, killBtn.graveStoneList);
    }

    public void ChangeButtonUI(bool active)
    {
        //활성화하기
        if (active)
        {
            reportButtonImage.color = normal;
            reportButton.interactable = true;
        }
        //비활성화하기
        else
        {
            reportButtonImage.color = transparent;
            reportButton.interactable = false;
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

    public void FindGraveStone(Vector3 posPlayer, List<GameObject> graves)
    {
        if(graves.Count != 0)
        {
            Vector3 posGrave;
            double[] distances = new double[graves.Count];
            double min;
            for (int i = 0; i < graves.Count; i++)
            {
                posGrave = graves[i].transform.position;
                distances[i] = calcDistance(posPlayer, posGrave);
            }
            nearbyObject = graves[0];
            min = distances[0];
            for (int i = 0; i < distances.Length; i++)
            {
                if(min > distances[i])
                {
                    nearbyObject = graves[i];
                    min = distances[i];
                }
            }
            if(min < 5.0f)
            {
                ChangeButtonUI(true);
                if (reportButton.onClick != null)
                {
                    reportButton.onClick.RemoveAllListeners();
                }
                reportButton.onClick.AddListener(() => Report_The_Grave(nearbyObject));
            }
            else
            {
                ChangeButtonUI(false);
            }
        }
    }

    public void Report_The_Grave(GameObject nearbyObject)
    {
        KillObject obj = nearbyObject.GetComponent<Kill_InteractObject>().killObject;
        string obj_Name = obj.objectName;
        Debug.Log("무덤 이름 : " + obj_Name);
    }
}