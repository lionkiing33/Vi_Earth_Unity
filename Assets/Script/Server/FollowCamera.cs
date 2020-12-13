using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    public Transform followtarget;
    private CinemachineVirtualCamera vcam;
    //Player다섯명의 정보 중 user찾기.
    public string MyID;
    public int imageNumber;

    public GameObject[] dead = new GameObject[5];
    public bool check = false;

    void OnEnable()
    {
        SceneDeliver userinfo = GameObject.Find("SceneDeliver").GetComponent<SceneDeliver>();
        MyID = userinfo.userID;
        for (int i = 0; i < userinfo.playerInfo.USER.Length; i++) if (MyID.Equals(userinfo.playerInfo.USER[i].UserID)) imageNumber = i+1;
    }
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.Find("Player"+imageNumber+"Object");
        followtarget = player.transform;
        vcam.Follow = followtarget;
    }

    void Update()
    {
        for(int i = 0; i < 5; i++)
        {
            if (player == dead[i] && check)
            {
                player = dead[i];
                followtarget = player.transform;
                vcam.Follow = followtarget;
                check = false;
            }
        }
    }
}
