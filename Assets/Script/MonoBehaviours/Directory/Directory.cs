using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//플레이어의 미션(퀘스트) 목록을 관리하고 미션(퀘스트) 목록 UI를 담당하는 스크립트
public class Directory : MonoBehaviour
{
    //Quest 프리팹
    public Quest questPrefab;
    //Quest 인스턴스 변수
    private Quest quest;
    //Quest 배열
    private const int numQuests = 4;
    private Quest[] quests = new Quest[numQuests];

    //10개의 미션 중 4개의 랜덤 미션 인덱스 배열
    private int[] randomIndex;

    //Quest 내 장소/내용/최소,최대갯수 텍스트 배열
    public Text[] locationTexts = new Text[numQuests];
    private Text[] contentTexts = new Text[numQuests];
    private Text[] numberTexts = new Text[numQuests];

    //상호작용 오브젝트 배열
    public const int numInteractObjects = 9;
    public InteractObject[] interactObjects = new InteractObject[numInteractObjects];

    //미션 목록 수직 레이어 그룹 게임오브젝트
    private GameObject directoryBackground;

    //미션 목록 버튼 크기
    private RectTransform buttonSize;
    //미션 목록 버튼 가로 최소 크기
    private float horizontalMinSize = 30.0f;
    //미션 목록 버튼 가로 최대 크기
    private float horizontalMaxSize = 410.0f;

    //미션 목록 버튼 위치
    private Vector3 buttonLocation;
    //미션 목록 버튼 x축 변화량
    private float xAxisVariation = 190.0f;

    //미션 목록 버튼 사용 유무
    private bool isitClicked = false;

    public void Start()
    {
        //미션 목록 수직 레이어 그룹 게임오브젝트 초기화
        directoryBackground = this.transform.GetChild(1).gameObject;

        //랜덤 미션 인덱스 배열 초기화
        randomIndex = getRandomInt(numQuests, 0, numInteractObjects);

        //상호작용 오브젝트 배열 내 속성값 초기화
        for (int i = 0; i < numInteractObjects; i++)
        {
            if(interactObjects[i].objectName == "Shelf")
            {

                interactObjects[i].min = 0;
                interactObjects[i].max = 1;
                interactObjects[i].isItUsable = true;
            }
            else
            {

                interactObjects[i].min = 0;
                interactObjects[i].max = 2;
                interactObjects[i].isItUsable = true;
            }
        }

        //퀘스트 생성함수 호출
        CreateQuests();

        //미션 목록 버튼 크기
        buttonSize = this.GetComponent<RectTransform>();
        //미션 목록 버튼 위치
        buttonLocation = this.transform.localPosition;
    }

    public void CreateQuests()
    {
        //Quest 프리팹을 사용하기 전에 유니티 에디터를 통해 설정했는지 확인
        if (questPrefab != null)
        {
            //Quest의 갯수만큼 반복문 실행
            for (int i = 0; i < numQuests; i++)
            {
                //Quest 프리팹의 복사본을 인스턴스화해서 quest에 대입한다
                quest = Instantiate(questPrefab);

                //인스턴스화한 오브젝트의 name 속성에 대입함
                quest.name = "Quest_#" + i;

                //DirectoryBackground오브젝트를 인스턴스화한 Quest의 부모 오브젝트로 설정함
                quest.transform.SetParent(directoryBackground.transform);

                //새로운 Quest 오브젝트를 quests 배열의 현재 인덱스에 대입함
                quests[i] = quest;

                //Quest의 자식 오브젝트 중 인덱스 0,1,2에 해당하는 자식 오브젝트는 LocationTexts, ContentTexts, QuantityTexts이다
                //플레이어가 아이템과 상호작용할 때 텍스트 컴포넌트의 텍스트가 일과표에 나타난다
                locationTexts[i] = quest.transform.GetChild(0).GetComponent<Text>();
                contentTexts[i] = quest.transform.GetChild(1).GetComponent<Text>();
                numberTexts[i] = quest.transform.GetChild(2).GetComponent<Text>();
            }

            //Quest의 갯수만큼 반복문 실행
            for (int i = 0; i < numQuests; i++)
            {
                for (int j = 0; j < numInteractObjects; j++)
                {
                    if (j == randomIndex[i])
                    {
                        quests[i].name = interactObjects[j].objectName;
                        interactObjects[j].isItUsable = false;

                        //미션 오브젝트의 location,content,quantity속성을 LocationTexts, ContentTexts, QuantityTexts 배열의 텍스트 오브젝트에 대입함
                        locationTexts[i].text = interactObjects[j].location;
                        contentTexts[i].text = interactObjects[j].content;
                        numberTexts[i].text = "(" + interactObjects[j].min.ToString() + "/" + interactObjects[j].max.ToString() + ")";

                        //위치,내용,최대수량 텍스트를 활성화
                        locationTexts[i].enabled = true;
                        contentTexts[i].enabled = true;
                        numberTexts[i].enabled = true;

                        break;
                    }
                }
            }
        }
    }

    public void AddQuest()
    {
        //Quest 프리팹을 사용하기 전에 유니티 에디터를 통해 설정했는지 확인
        if (questPrefab != null)
        {
            quest = Instantiate(questPrefab);
            quest.name = "Quest_#" + numQuests;
            quest.transform.SetParent(directoryBackground.transform);

            //Quest의 갯수만큼 반복문 실행
            for (int i = 0; i < numQuests; i++)
            {
                //Quest 프리팹의 복사본을 인스턴스화해서 quest에 대입한다
                quest = Instantiate(questPrefab);

                //인스턴스화한 오브젝트의 name 속성에 대입함
                quest.name = "Quest_#" + i;

                //DirectoryBackground오브젝트를 인스턴스화한 Quest의 부모 오브젝트로 설정함
                quest.transform.SetParent(directoryBackground.transform);

                //새로운 Quest 오브젝트를 quests 배열의 현재 인덱스에 대입함
                quests[i] = quest;

                //Quest의 자식 오브젝트 중 인덱스 0,1,2에 해당하는 자식 오브젝트는 LocationTexts, ContentTexts, QuantityTexts이다
                //플레이어가 아이템과 상호작용할 때 텍스트 컴포넌트의 텍스트가 일과표에 나타난다
                locationTexts[i] = quest.transform.GetChild(0).GetComponent<Text>();
                contentTexts[i] = quest.transform.GetChild(1).GetComponent<Text>();
                numberTexts[i] = quest.transform.GetChild(2).GetComponent<Text>();
            }

            //Quest의 갯수만큼 반복문 실행
            for (int i = 0; i < numQuests; i++)
            {
                for (int j = 0; j < numInteractObjects; j++)
                {
                    if (j == randomIndex[i])
                    {
                        quests[i].name = interactObjects[j].objectName;
                        interactObjects[j].isItUsable = false;

                        //미션 오브젝트의 location,content,quantity속성을 LocationTexts, ContentTexts, QuantityTexts 배열의 텍스트 오브젝트에 대입함
                        locationTexts[i].text = interactObjects[j].location;
                        contentTexts[i].text = interactObjects[j].content;
                        numberTexts[i].text = "(" + interactObjects[j].min.ToString() + "/" + interactObjects[j].max.ToString() + ")";

                        //위치,내용,최대수량 텍스트를 활성화
                        locationTexts[i].enabled = true;
                        contentTexts[i].enabled = true;
                        numberTexts[i].enabled = true;

                        break;
                    }
                }
            }
        }
    }

    //미션이 수행되고 해당 미션오브젝트의 정보를 토대로 일과표를 잘 수정하였는지 나타내는 bool 값을 반환함
    public bool ModifyQuest(InteractObject additional)
    {
        for (int i = 0; i < numQuests; i++)
        {
            //모든 조건이 맞으면 원래 있던 미션(퀘스트)의 갯수 및 UI를 변경함
            if (quests[i].name == additional.objectName && additional.isItUsable == false)
            {
                //일과표 내 해당 미션(퀘스트)의 수행한 갯수를 1 증가시킨다
                additional.min += 1;

                //Quest 프리팹을 인스턴스화하면 Quest 스크립트가 들어있는 게임 오브젝트가 만들어진다.
                //이 코드는 Quest 스크립트의 참조를 얻는 코드다 
                //Quest 스크립트에는 Text형식의 자식오브젝트인 taskLocationText, taskContentText, maxTaskText가 들어있다
                Quest questScript = quests[i].gameObject.GetComponent<Quest>();

                //Text오브젝트의 참조를 저장함
                Text locationText = questScript.locationText;
                Text contentText = questScript.contentText;
                Text numberText = questScript.numberText;

                //해당 미션(퀘스트)의 남은 갯수가 1개라도 남아있으면 텍스트를 노란색으로 설정
                if (additional.min < additional.max)
                {
                    //Text오브젝트의 text속성 설정함
                    locationText.text = "<color=#ffff00>" + additional.location + "</color>";
                    contentText.text = "<color=#ffff00>" + additional.content + "</color>";
                    numberText.text = "<color=#ffff00>(" + additional.min.ToString() + "/" + additional.max.ToString() + ")</color>";
                    return false;
                }
                //해당 미션(퀘스트)의 남은 갯수가 0이 되면 택스트를 초록색으로 설정
                else
                {
                    additional.isItUsable = true;
                    //Text오브젝트의 text속성 설정함
                    locationText.text = "<color=#00ff00>" + additional.location + "</color>";
                    contentText.text = "<color=#00ff00>" + additional.content + "</color>";
                    numberText.text = "<color=#00ff00>(" + additional.min.ToString() + "/" + additional.max.ToString() + ")</color>";
                    return true;
                }
            }
        }
        return false;
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

    //일과표 버튼 UI 변경 함수
    public void changeScale()
    {
        //버튼이 사용되지 않은 경우
        if (!isitClicked)
        {
            //크기 변경
            buttonSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalMinSize);
            //위치 변경
            buttonLocation.x -= xAxisVariation;
            this.transform.localPosition = buttonLocation;
            //수직 레이어 그룹 비활성화
            directoryBackground.SetActive(false);
            //버튼 사용됨 설정
            isitClicked = true;
        }
        //버튼이 사용된 경우
        else
        {
            //크기 변경
            buttonSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalMaxSize);
            //위치 변경
            buttonLocation.x += xAxisVariation;
            this.transform.localPosition = buttonLocation;
            //수직 레이어 그룹 활성화
            directoryBackground.SetActive(true);
            //버튼 사용되지 않음 설정
            isitClicked = false;
        }
    }
}