using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class SocketTest_player3 : MonoBehaviour
{
    private float x=0, y = -8;
    public GameObject example;
	public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        SocketManger.Socket.On("sendTomove3", (data) => {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            Player3Move response = JsonUtility.FromJson<Player3Move>(json);
            x = response.x;
            y = response.y;
			check = true;
        });
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
		if(check){
			Vector3 Pos;
			//Debug.Log("number와 1은 같습니다.");
			//예시로 서버에서 이미지값이 1을 보냈을 때 2도 움직이게 설정
			Pos = example.transform.localPosition;
			Pos.x = x;
			Pos.y = y;
			example.transform.localPosition = Pos;
			check = false;
		}
    }
    private class Player3Move
    {
        public float x, y;
    }
}
