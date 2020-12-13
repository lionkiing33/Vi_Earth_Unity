using UnityEngine;

[CreateAssetMenu(menuName = "KillObject")]

public class KillObject : ScriptableObject
{
    //오브젝트의 이름
    public string objectName;

    //오브젝트의 이미지
    public Sprite sprite;

    //오브젝트의 위치
    public Vector3 location;

    public bool IsPlayerAlive = true;

    //오브젝트의 종류를 나타내는 열거형 상수 정의(오브젝트 식별)
    public enum ObjectType
    {
        Player1Object,
        Player2Object,
        Player3Object,
        Player4Object,
        Player5Object
    }

    //ObjectType 열거 형식을 사용하는 objectType 속성
    public ObjectType objectType;
}
