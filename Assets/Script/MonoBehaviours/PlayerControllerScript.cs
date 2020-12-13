using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

//플레이어가 이동중 collider ststic인 물체와 부딪혔을때 플레이어는 지나가지 못하게 보이지만 플레이어 좌표만 움직이던것을 개선함
//미니플레이어는 미니맵 내 플레이어 위치를 나타내주는 객체이므로 플레이어의 축소판입니다.
//플레이어의 백터(방향)는 미니플레이어와 동일하게 사용할수있으나 속도,좌표는 다르게 사용되어야합니다
//플레이어는 패널(미니게임, 미니맵 등)에 영향을 받으면 안됩니다. 또한 미니플레이어는 게임 내 지형 및 오브젝트와 다른 패널에 영향을 받으면 안됩니다.
//플레이어와  미니플레이어 간 collider는 layer를 통해 구별하였습니다.
//그러나 남은 문제점이라고 하면 1) 미니플레이어는 외부 지형에 영향을 받지 않기 때문에 플레이어가 물체와 부딪혀도 미니플레이어는 계속 지나가며 좌표가 움직이게 됩니다
//2) 미니맵과 현재 진행 사이의 좌표 비율을 계산하여 미니플레이어가 적절한 속도로 움직이게끔 보여줘야합니다(속도는 변경이 안되고 Vector3를 바꿔야합니다.)

public class PlayerControllerScript : MonoBehaviour
{
    //조이스틱 스크립트
    public Joystick joystick;
    public GameObject Player;
    public string MyID;
    public int imageNumber;
    public static bool playerController_killed_check = false;
    public bool check = false;

    //플레이어 이동속도
    float playerMoveSpeed = 9.0f;

    //플레이어의 이동백터
    Vector3 playerMoveVector = new Vector3();

    //플레이어 이동 애니메이션
    Animator animator;

    //플레이어 이동 애니메이션 문자열 설정
    string animationState = "AnimationState";

    //플레이어 물체 설정
    Rigidbody2D playerRB2D;

    public string ventState = "Standing";

    private IEnumerator coroutine;

    enum CharStates
    {
        walkEast = 1,
        walkWest = 2,
        standing = 3,
        ventUpDown = 4,
        ventDownUp = 5
    }

    [System.Serializable]
    public class Data_Move_x_y
    {
        public int image_count;
        public float x;
        public float y;
    }
    [System.Serializable]
    public class Count_number
    {
        public int image_count;
    }

    void OnEnable()
    {
        SceneDeliver userinfo = GameObject.Find("SceneDeliver").GetComponent<SceneDeliver>();
        MyID = userinfo.userID;
        for (int i = 0; i < userinfo.playerInfo.USER.Length; i++) if (MyID.Equals(userinfo.playerInfo.USER[i].UserID)) imageNumber = i + 1;
        check = true;
    }

    private void Start()
    {
        //플레이어 설정
        playerMoveVector = Vector3.zero;  //플레이어 이동벡터 초기화
        animator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(check)
        {
            //터치패드 입력 받기
            HandleInput();
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (ventState != "Up To Down" && ventState != "Down To Up")
        {
            UpdateState();
        }
    }

    public void HandleInput()
    {
        //조이스틱으로 전달받은 X,Y 벡터값을 저장
        playerMoveVector = PoolInput();
    }

    //조이스틱으로 전달받은 X,Y 벡터값을 반환함
    public Vector3 PoolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(h, v, 0).normalized;

        return moveDir;
    }

    //플레이어 이동
    public void Move()
    {
        //플레이어의 이동 방향(벡터),이동 속도,이동 시간을 곱하여 해당 방향에 따른 이동 거리를 계산하여 이동합니다.
        playerRB2D.velocity = playerMoveVector * playerMoveSpeed;
        if (playerMoveVector.x != 0 || playerMoveVector.y != 0)
        {
            if (playerController_killed_check)
            {
                Debug.Log("죽어서 데이터 안보냄");
                Debug.Log(playerController_killed_check);
            }
            else
            {
                var new_data = new Data_Move_x_y();
                new_data.image_count = imageNumber; //유저 이미지 정보
                Vector3 CPos = Player.transform.localPosition;
                new_data.x = CPos.x;
                new_data.y = CPos.y;
                string message = JsonUtility.ToJson(new_data, prettyPrint: true);
                string temp = "move" + imageNumber;
                SocketManger.Socket.Emit(temp, message);
            }
        }
    }

    private void UpdateState()
    {
        //현재의 플레이어 x좌표가 이전의 플레이어 x좌표보다 상승했을경우
        if (playerMoveVector.x > 0)
        {
            //플레이어의 이동 애니메이션을 동쪽이동으로 설정
            animator.SetInteger(animationState, (int)CharStates.walkEast);
        }
        //현재의 플레이어 x좌표가 이전의 플레이어 x좌표보다 감소했을경우
        else if (playerMoveVector.x < 0)
        {
            //플레이어의 이동 애니메이션을 서쪽이동으로 설정
            animator.SetInteger(animationState, (int)CharStates.walkWest);
        }
        //현재의 플레이어 좌표와 이전의 플레이어 좌표가 동일한 경우
        else
        {
            //플레이어의 이동 애니메이션을 제자리로 설정
            animator.SetInteger(animationState, (int)CharStates.standing);
        }
    }
}