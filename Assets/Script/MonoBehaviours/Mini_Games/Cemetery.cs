using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cemetery : MonoBehaviour
{
    //공동묘지 미니게임 패널 게임오브젝트
    private GameObject cemetery;
    //관짝 게임오브젝트
    private GameObject target;
    //관뚜껑 위치
    private Vector3 coffinPosition;
    private Vector3 first;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject coffin;

    // Start is called before the first frame update
    void Start()
    {
        cemetery = this.transform.parent.transform.parent.gameObject;
        target = this.transform.parent.transform.GetChild(0).gameObject;
        coffinPosition = this.transform.localPosition;
        mini_game = cemetery.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
    }

    //처음 클릭했을때 이벤트
    public void firstdrag(GameObject Img)
    {
        //처음 좌표 저장.
        first = Img.transform.position;
    }

    //드래그중일때 이벤트
    public void Ondrag(GameObject Img)
    {
        //클릭한 오브젝트 움직이기.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        Img.transform.position = mousePos;
    }

    //드래그 끝났을때 이벤트
    public void enddrag(GameObject Img)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        float x = target.transform.position.x - Img.transform.position.x;
        float y = target.transform.position.y - Img.transform.position.y;
        if (x < 1 && x > -1 && y < 1 && y > -1)
        {
            Invoke("Exit_Panel", 1.0f);
        }
    }

    public void Exit_Panel()
    {
        cemetery.SetActive(false);
        mini_game.ClearMiniGame(coffin);
        if (coffin.min < coffin.max)
        {
            this.transform.localPosition = coffinPosition;
        }
    }
}