using UnityEngine;

[CreateAssetMenu(menuName = "VentObject")]

public class VentObject : ScriptableObject
{
    //오브젝트의 이름
    public string objectName;

    //오브젝트의 이미지
    public Sprite sprite;

    //오브젝트의 위치
    public Vector3 location;

    //오브젝트의 종류를 나타내는 열거형 상수 정의(오브젝트 식별)
    public enum ObjectType
    {
        Mart,
        ParkingLot,
        Hospital,
        Library,
        Cemetery,
        Security,
        LakeUpper,
        LakeLower,
        Basketball
    }

    //ObjectType 열거 형식을 사용하는 objectType 속성
    public ObjectType objectType;
}
