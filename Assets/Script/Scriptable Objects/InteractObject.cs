using UnityEngine;

//생성 메뉴 안에 "Object"라는 하위 메뉴를 만들어 준다
//메뉴를 통해 스크립팅 가능한 오브젝트인 "Object"의 인스턴스를 쉽게 만들 수 있다
[CreateAssetMenu(menuName = "InteractObject")]

//플레이어가 퀘스트를 수행하기 위해 상호작용을 일으키는 오브젝트의 데이터를 저장할 "Object"
public class InteractObject : ScriptableObject
{
    //오브젝트의 이름
    public string objectName;

    //오브젝트의 이미지
    public Sprite sprite;

    //오브젝트의 위치
    public string location;

    //오브젝트의 퀘스트 내용
    public string content;

    //오브젝트의 최소 갯수
    public int min;
    //오브젝트의 최대 갯수
    public int max;

    //퀘스트 완료시 증가되는 퀘스트 진행 게이지 양
    public float taskPoints;

    //오브젝트의 사용 유무
    public bool isItUsable;

    //오브젝트의 종류를 나타내는 열거형 상수 정의(오브젝트 식별)
    public enum ObjectType
    {
        COIN,
        GAS_STATION,
        VACCINE,
        BOOK,
        COFFIN,
        BASKETBALL_HOOP,
        SAFE,
        SHELF,
        POWER_SUPPLY,
        FINGERPRINT,
        LAMP,
        BELL
    }

    //ObjectType 열거 형식을 사용하는 objectType 속성
    public ObjectType objectType;
}