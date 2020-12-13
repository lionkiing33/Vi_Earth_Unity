using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mart : MonoBehaviour
{
    //마트 미니게임 패널 게임오브젝트
    private GameObject mart;
    //식료품 게임 오브젝트 배열 생성
    private const int numOfFood = 10;
    //식료품 이미지 배열 생성
    private Image[] foodImage = new Image[numOfFood];
    //식료품 스프라이트 배열 생성
    public Sprite[] foodSprite = new Sprite[numOfFood];
    //식료품 문자열 배열 생성
    private string[] foodName = new string[numOfFood];
    //식료품 텍스트 배열 생성
    private Text[] foodText = new Text[numOfFood];
    //식료품 버튼 배열 생성
    private Button[] foodButton = new Button[numOfFood];

    //쇼핑리스트 미션 내용 게임오브젝트 배열 생성
    private const int numOfContent = 3;
    //쇼핑리스트 미션 내용 텍스트 배열 생성
    private Text[] contentText = new Text[numOfContent];

    //쇼핑리스트 미션 체크 이미지 배열 생성
    private Image[] checkImage = new Image[numOfContent];
    //쇼핑리스트 미션 체크 스프라이트 생성
    public Sprite checkSprite;

    //카트 내 식료품 게임오브젝트 배열 생성
    private Image[] foodInCartImage = new Image[numOfContent];

    //10개의 foodImage 중 활성화할 foodImage의 인덱스 랜덤 배열
    private int[] foodImageIndex;
    //10개의 foodSprite 중 활성화할 foodSprite의 인덱스 랜덤 배열
    private int[] foodSpriteIndex;
    //10개의 foodSprite 중 모든 foodSprite의 인덱스 랜덤 배열
    private int[] contentTextIndex;
    private bool[] isSame;

    //컬러 객체
    private Color normal = new Color(255, 255, 255, 255);
    private Color transparent = new Color(255, 255, 255, 0);

    //전체적인 미니게임 관리하는 스크립트
    private Mini_Game mini_game;
    //해당 미션 오브젝트
    public InteractObject shelf;

    // Start is called before the first frame update
    void Start()
    {
        mart = this.transform.parent.gameObject;
        for (int i = 0; i < numOfFood; i++)
        {
            foodText[i] = this.transform.GetChild(0).transform.GetChild(i + 3).transform.GetChild(0).GetComponent<Text>();
            foodImage[i] = this.transform.GetChild(0).transform.GetChild(i + 3).GetComponent<Image>();
            foodButton[i] = this.transform.GetChild(0).transform.GetChild(i + 3).GetComponent<Button>();
        }

        for (int i = 0; i < numOfContent; i++)
        {
            contentText[i] = this.transform.GetChild(0).transform.GetChild(14).transform.GetChild(2 * i + 1).gameObject.GetComponent<Text>();
            foodInCartImage[i] = this.transform.GetChild(0).transform.GetChild(i).gameObject.GetComponent<Image>();
            checkImage[i] = this.transform.GetChild(0).transform.GetChild(14).transform.GetChild(2 * i + 2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
        }
        mini_game = mart.transform.GetChild(0).gameObject.GetComponent<Mini_Game>();

        Init_Food();
        Init_Content();
        foodImageIndex = getRandomInt(5, 0, 10);
        foodSpriteIndex = getRandomInt(5, 0, 10);
        Update_Food(foodImageIndex, foodSpriteIndex);
        Update_Content(foodSpriteIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkImage[0].sprite == checkSprite && checkImage[1].sprite == checkSprite && checkImage[2].sprite == checkSprite)
        {
            Exit_Panel();
        }
    }

    public void Init_Food()
    {
        for (int i = 0; i < numOfFood; i++)
        {
            foodName[i] = foodText[i].text;
            foodText[i].text = null;

            foodImage[i].color = transparent;
            foodImage[i].sprite = null;

            foodButton[i].interactable = false;
        }
    }

    public void Init_Content()
    {
        for (int i = 0; i < numOfContent; i++)
        {
            contentText[i].text = null;

            checkImage[i].color = transparent;
            checkImage[i].sprite = null;

            foodInCartImage[i].color = transparent;
            foodInCartImage[i].sprite = null;
        }
    }

    public void Update_Food(int[] randomImage, int[] randomSprite)
    {
        for (int i = 0; i < randomImage.Length; i++)
        {
            foodImage[randomImage[i]].color = normal;
            foodImage[randomImage[i]].sprite = foodSprite[randomSprite[i]];
            foodButton[randomImage[i]].interactable = true;
            foodText[randomImage[i]].text = foodName[randomSprite[i]];
        }
    }

    public void Update_Content(int[] randomSprite)
    {
        contentTextIndex = new int[numOfContent];
        isSame = new bool[numOfContent];

        for (int i = 0; i < numOfContent;)
        {
            contentTextIndex[i] = Random.Range(0, 10);
            for (int k = 0; k < i;)
            {
                if (contentTextIndex[k] == contentTextIndex[i])
                {
                    contentTextIndex[i] = Random.Range(0, 10);
                }
                else
                {
                    k++;
                }
            }
            isSame[i] = false;
            for (int j = 0; j < randomSprite.Length; j++)
            {
                if (contentTextIndex[i] == randomSprite[j])
                {
                    isSame[i++] = true;
                    break;
                }
            }
        }
        for (int i = 0; i < numOfContent; i++)
        {
            contentText[i].text = foodName[contentTextIndex[i]];
            //Debug.Log(contentTextIndex[0] + "," + contentTextIndex[1] + "," + contentTextIndex[2]);
        }
    }

    public void Choose_Food(int index)
    {
        for (int i = 0; i < numOfContent; i++)
        {
            if(foodInCartImage[i].sprite == null)
            {
                for (int j = 0; j < numOfContent; j++)
                {
                    if(foodText[index].text == contentText[j].text)
                    {
                        for (int k = 0; k < foodSpriteIndex.Length; k++)
                        {
                            if (foodImage[index].sprite == foodSprite[foodSpriteIndex[k]])
                            {
                                foodInCartImage[i].color = normal;
                                foodInCartImage[i].sprite = foodSprite[foodSpriteIndex[k]];

                                checkImage[j].color = normal;
                                checkImage[j].sprite = checkSprite;

                                foodImage[index].color = transparent;
                                foodImage[index].sprite = null;

                                break;
                            }
                        }
                        break;
                    }
                }
                break;
            }
        }
    }

    //범위 내 중복없는 난수 발생기
    //max포함안함 => (length)<(max-min)이어야함
    public int[] getRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;
        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;
                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }

    public void Exit_Panel()
    {
        mini_game.ClearMiniGame(shelf);
        mart.SetActive(false);
    }
}