using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillButton : MonoBehaviour
{
    //플레이어 게임오브젝트
    public GameObject player;

    //죽일 크루원 게임오브젝트 배열
    public const int numOfCitizen = 4;
    public GameObject[] citizen = new GameObject[numOfCitizen];
    public List<GameObject> kill = new List<GameObject>();
    //죽일 크루원 오브젝트 배열
    private KillObject[] kill_interactions = new KillObject[numOfCitizen];

    //무덤 프리팹
    public GameObject graveStonePrefab;
    //무덤 인스턴스 변수
    private GameObject graveStone;
    //무덤 리스트
    public List<GameObject> graveStoneList = new List<GameObject>();

    //조이스틱 게임오브젝트
    public GameObject joystick;

    //죽일 크루원과의 플레이어 사이 거리 double 배열
    private double[] distances;
    private double min;

    //플레이어와 제일 가까운 죽일 크루원 게임오브젝트
    public GameObject nearbyObject;

    //죽일 크루원 게임오브젝트 이미지 객체
    private SpriteOutline killBorder;

    //킬 버튼
    private Button killButton;
    //킬 버튼 이미지
    private Image killButtonImage;
    //킬 버튼 마스크 이미지
    private Image killButtonBackgroundImage;
    //킬 타이머 텍스트
    private Text killButtonText;
    //킬 타이머 남은시간
    private float currentCoolTime;
    //킬 타이머 쿨타임
    private float coolTime = 10f;

    //쿨타임 이미지 구현 코르틴
    private IEnumerator coolTimeImage;
    //쿨타임 타이머 구현 코르틴
    private IEnumerator coolTimeText;

    //컬러 객체
    private Color normal = new Color(255, 255, 255, 255);
    private Color transparent = new Color(255, 255, 255, 0.5f);

    public bool isItStarted = false;

    /*
    // Start is called before the first frame update
    void OnEnable ()
    {
        for (int i = 0; i < numOfCitizen; i++)
        {
            kill_interactions[i] = citizen[i].GetComponent<Kill_InteractObject>().killObject;
            //플레이어가 살아있는지 확인하는 bool 변수 초기화
            kill_interactions[i].IsPlayerAlive = true;
            kill.Add(citizen[i]);
        }
        //킬 버튼 초기화
        killButton = this.GetComponent<Button>();
        //킬 버튼 이미지 초기화
        killButtonImage = this.GetComponent<Image>();
        ChangeButtonUI(false);

        //킬 버튼 마스크 이미지 초기화
        killButtonBackgroundImage = this.transform.GetChild(0).GetComponent<Image>();
        killButtonBackgroundImage.fillAmount = 0;
        //킬 버튼 타이머 초기화
        killButtonText = this.transform.GetChild(1).GetComponent<Text>();
        killButtonText.text = null;
        //킬 타이머 남은시간 초기화
        currentCoolTime = 0f;
        //코르틴 초기화
        coolTimeImage = Cooltime();
        coolTimeText = CoolTimeCounter();
    }
    */
    // Update is called once per frame
    void Update()
    {
        if(isItStarted)
        {
            for (int i = 0; i < numOfCitizen; i++)
            {
                kill_interactions[i] = citizen[i].GetComponent<Kill_InteractObject>().killObject;
                //플레이어가 살아있는지 확인하는 bool 변수 초기화
                kill_interactions[i].IsPlayerAlive = true;
                kill.Add(citizen[i]);
            }
            //킬 버튼 초기화
            killButton = this.GetComponent<Button>();
            //킬 버튼 이미지 초기화
            killButtonImage = this.GetComponent<Image>();
            ChangeButtonUI(false);

            //킬 버튼 마스크 이미지 초기화
            killButtonBackgroundImage = this.transform.GetChild(0).GetComponent<Image>();
            killButtonBackgroundImage.fillAmount = 0;
            //킬 버튼 타이머 초기화
            killButtonText = this.transform.GetChild(1).GetComponent<Text>();
            killButtonText.text = null;
            //킬 타이머 남은시간 초기화
            currentCoolTime = 0f;
            //코르틴 초기화
            coolTimeImage = Cooltime();
            coolTimeText = CoolTimeCounter();
            isItStarted = false;
        }
        else
        {
            Update_KillList();
            //플레이어의 위치에 따른 미션오브젝트 리스트 내역 거리 비교 및 UI변경
            Vector3 posPlayer = player.gameObject.transform.position;
            btwPlayerAndkill(posPlayer, kill);
        }
    }

    public void Update_KillList()
    {
        for (int i = 0; i < numOfCitizen; i++)
        {
            if (kill_interactions[i].IsPlayerAlive == false)
            {
                for (int j = 0; j < kill.Count; j++)
                {
                    if (kill_interactions[i].objectName == kill[j].GetComponent<Kill_InteractObject>().killObject.objectName)
                    {
                        kill.RemoveAt(j);
                    }
                }
            }
        }
    }

    public void ChangeButtonUI(bool active)
    {
        //활성화하기
        if (active)
        {
            killButtonImage.color = normal;
            killButton.interactable = true;
        }
        //비활성화하기
        else
        {
            killButtonImage.color = transparent;
            killButton.interactable = false;
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
    public void btwPlayerAndkill(Vector3 posPlayer, List<GameObject> kills)
    {
        if (kills.Count != 0)
        {
            Vector3 poskill;
            distances = new double[kills.Count];
            for (int i = 0; i < kills.Count; i++)
            {
                poskill = kills[i].transform.position;
                distances[i] = calcDistance(posPlayer, poskill);
                if (distances[i] > 5f)
                {
                    //오브젝트 태두리 설정
                    killBorder = kills[i].GetComponent<SpriteOutline>();
                    killBorder.enabled = false;
                }
                else
                {
                    //오브젝트 태두리 설정
                    killBorder = kills[i].GetComponent<SpriteOutline>();
                    killBorder.enabled = true;
                }
            }
            nearbyObject = kills[0];
            min = distances[0];
            for (int i = 0; i < distances.Length; i++)
            {
                if (min > distances[i])
                {
                    nearbyObject = kills[i];
                    min = distances[i];
                }
            }
            if (min < 5.0f)
            {
                KillObject paramObject = nearbyObject.gameObject.GetComponent<Kill_InteractObject>().killObject;

                GameObject killobject = GameObject.Find(paramObject.objectName);

                if (paramObject.IsPlayerAlive)
                {
                    if (killButton.onClick != null)
                    {
                        killButton.onClick.RemoveAllListeners();
                    }
                    killButton.onClick.AddListener(() => Use_kill(killobject, paramObject));
                }
                ChangeButtonUI(true);
            }
            else
            {
                ChangeButtonUI(false);
            }
        }
        else
        {
            //게임 종료
        }
    }

    public void Use_kill(GameObject ghost, KillObject ghostObject)
    {
        killInfo killinfo = new killInfo();
        Vector3 pos;
        pos = ghost.transform.localPosition;
        if (ghostObject.objectName.Equals("Player1Object"))
        {
            killinfo.ImageNumber = 1;
        }
        else if (ghostObject.objectName.Equals("Player2Object"))
        {
            killinfo.ImageNumber = 2;
        }
        else if (ghostObject.objectName.Equals("Player3Object"))
        {
            killinfo.ImageNumber = 3;
        }
        else if (ghostObject.objectName.Equals("Player4Object"))
        {
            killinfo.ImageNumber = 4;
        }
        else if (ghostObject.objectName.Equals("Player5Object"))
        {
            killinfo.ImageNumber = 5;
        }

        killinfo.x = pos.x;
        killinfo.y = pos.y;

		Debug.Log(killinfo.ImageNumber);
		Debug.Log(killinfo.x);
		Debug.Log(killinfo.y);
        string message = JsonUtility.ToJson(killinfo, prettyPrint: true);
        SocketManger.Socket.Emit("kill", message);

		GameObject temp_player = GameObject.Find("Player" + killinfo.ImageNumber + "Object");
		temp_player.SetActive(false);
		//무덤 프리팹 인스턴스 생성
        graveStone = Instantiate(graveStonePrefab);
        //죽은 플레이어의 킬 인터렉트 속성 값을 무덤 오브젝트의 킬 인터렉트 속성에 저장함
        graveStone.GetComponent<Kill_InteractObject>().killObject = temp_player.GetComponent<Kill_InteractObject>().killObject;
        //무덤의 위치를 죽은 플레이어의 위치로 이동
        graveStone.transform.localPosition = new Vector3(killinfo.x, killinfo.y, 0);
         //무덤 오브젝트를 무덤 리스트에 추가함
         graveStoneList.Add(graveStone);

        //플레이어가 죽여서 죽은 플레이어의 위치로 이동
        player.transform.localPosition = ghost.transform.localPosition;
        /*
        //죽은 플레이어의 에니메이터 설정
        Animator deadPlayerAnimator = ghost.GetComponent<Animator>();
        deadPlayerAnimator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Ani\\" + ghostObject.objectName + "GhostController", typeof(RuntimeAnimatorController)));

        //죽은 플레이어의 컬러 설정
        SpriteRenderer deadPlayerSprite = ghost.GetComponent<SpriteRenderer>();
        deadPlayerSprite.sprite = ghostObject.sprite;

        //죽은 플레이어의 태두리 설정
        SpriteOutline deadPlayerBorder = ghost.GetComponent<SpriteOutline>();
        deadPlayerBorder.enabled = false;
        //죽은 플레이어의 레이어 설정
        ghost.layer = 0;
        //플레이어가 죽었다고 설정
        ghostObject.IsPlayerAlive = false;
        */
        /*
        //무덤 프리팹 인스턴스 생성
        graveStone = Instantiate(graveStonePrefab);
        //죽은 플레이어의 킬 인터렉트 속성 값을 무덤 오브젝트의 킬 인터렉트 속성에 저장함
        graveStone.GetComponent<Kill_InteractObject>().killObject = ghost.GetComponent<Kill_InteractObject>().killObject;
        //무덤의 위치를 죽은 플레이어의 위치로 이동
        graveStone.transform.localPosition = ghost.transform.localPosition;
        //무덤 오브젝트를 무덤 리스트에 추가함
        graveStoneList.Add(graveStone);
        */

        //타이머 마스크를 가림
        killButtonBackgroundImage.fillAmount = 1;
        killButton.enabled = false;

        StartCoroutine(coolTimeImage);

        //남은시간을 쿨타임시간으로 초기화함
        currentCoolTime = coolTime;
        killButtonText.text = "" + currentCoolTime;

        StartCoroutine(coolTimeText);
    }

    //킬 쿨타임 이미지 변경
    IEnumerator Cooltime()
    {
        //킬 쿨타임 마스크 조절
        while (killButtonBackgroundImage.fillAmount > 0)
        {
            killButtonBackgroundImage.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }
        yield break;
    }

    //킬 타이머 텍스트 변경
    IEnumerator CoolTimeCounter()
    {
        //킬 타이머 남은 시간 조절
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime -= 1.0f;
            killButtonText.text = "" + currentCoolTime;
        }

        //버튼 활성화
        killButton.enabled = true;
        //버튼 이미지 보이게함
        killButtonBackgroundImage.fillAmount = 0;
        //버튼 타이머 안보이게함
        killButtonText.text = null;

        ChangeButtonUI(true);

        coolTimeImage = Cooltime();
        coolTimeText = CoolTimeCounter();

        yield break;
    }
}
public class killInfo { 
    public int ImageNumber;
    public float x;
    public float y;
}