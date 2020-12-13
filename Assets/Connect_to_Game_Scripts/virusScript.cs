using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//바이러스 UI 이미지를 움직이는 script
public class virusScript : MonoBehaviour
{
    public Transform desPos, startPos, endPos;//목적지, 시작, 끝 위치 설정
    public int speed;//이동속도 설정
    public float startTime;//시작 시간 설정
    void Start()
    {
        transform.position = startPos.position;//시작위치 초기화
        desPos = endPos;//목적지 초기화
    }

    void FixedUpdate()
    {
        if (Time.time > startTime)//시간이 지정한 startTime보다 많아지면 시작
        {
            //y위치를 계속 변환. 속도는 지정한 값을 곱해준다.
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * speed);
            if(Vector2.Distance(transform.position, desPos.position) <= 0.05f)//위치가 목적지에 0.05만큼 가까워지면
            {
                transform.position = startPos.position;//오브젝트 위치를 시작위치로 초기화
            }
        }
    }
}
