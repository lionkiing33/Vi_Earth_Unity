using UnityEngine;
using UnityEngine.UI;

//퀘스트 안에서 사용할 텍스트 오브젝트의 참조를 저장하는 간단한 스크립트
public class Quest : MonoBehaviour
{
    //해당 퀘스트를 수행할 위치
    public Text locationText;
    //해당 퀘스트의 내용
    public Text contentText;
    //해당 퀘스트의 갯수(초기 수행 갯수 / 최대 수행 갯수)
    public Text numberText;
}
