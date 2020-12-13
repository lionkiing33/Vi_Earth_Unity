using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Game_Player : MonoBehaviour
{
    private int move_method;
    private float speed;
    private Vector2 speed_vec;
    private Vector3 miniPlayerPos;

    private GameObject MiniGames_Panel;
    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject coin;

    // Start is called before the first frame update
    void Start()
    {
        move_method = 2;
        speed = 5.0f;
        speed_vec = Vector2.zero;
        MiniGames_Panel = this.transform.parent.gameObject;
        //게임 시작 했을때 미니게임 플레이어의 좌표 초기화
        miniPlayerPos = this.gameObject.transform.position;
        mini_game = MiniGames_Panel.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 게임 오브젝트의 태그이름이 "Item"인 경우
        if (collision.CompareTag("Item"))
        {
            Exit_Panel();
        }
        //충돌한 게임 오브젝트의 태그이름이 "Enemy"인 경우
        else if (collision.CompareTag("Enemy"))
        {
            //미니게임 플레이어의 좌표 초기화
            this.transform.position = miniPlayerPos;
        }
    }

    // Update is called once per frame
    void Update()//방향키를 누르는 대로 움직인다.
    {
        if (move_method == 0)
        {
            speed_vec = Vector2.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                speed_vec.x += speed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                speed_vec.x -= speed;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                speed_vec.y += speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                speed_vec.y -= speed;
            }
            transform.Translate(speed_vec);
        }

        else if (move_method == 1) //관성의 법칙을 적용하기 떄문에 움직일 때 부드러움이 추가된다.
        {
            speed_vec.x = Input.GetAxis("Horizontal") * speed;
            speed_vec.y = Input.GetAxis("Vertical") * speed;

            transform.Translate(speed_vec);
        }

        else if (move_method == 2)
        {
            speed_vec = Vector2.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                speed_vec.x += speed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                speed_vec.x -= speed;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                speed_vec.y += speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                speed_vec.y -= speed;
            }

            GetComponent<Rigidbody2D>().velocity = speed_vec;
        }
    }

    public void Exit_Panel()
    {
        //패널 비활성화
        MiniGames_Panel.SetActive(false);
        //미션 종료 이후 업데이트
        mini_game.ClearMiniGame(coin);
        if (coin.min < coin.max)
        {
            //미니게임 플레이어의 좌표 초기화
            this.transform.position = miniPlayerPos;
        }
    }
}