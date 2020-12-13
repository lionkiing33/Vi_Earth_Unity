using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScript : MonoBehaviour
{
    public static string[] PlayerName = new string[5];
    public static int[] PlayerCharacter = new int[5];
    public static bool PlayerisImposter;
    public string MyID;
    public int imageNumber;

    void OnEnable()
    {
        SceneDeliver userinfo = GameObject.Find("SceneDeliver").GetComponent<SceneDeliver>();
        MyID = userinfo.userID;
        for(int i =0; i<5; i++)
        {
            PlayerName[i] = userinfo.playerInfo.USER[i].UserName;
            PlayerCharacter[i] = userinfo.playerInfo.USER[i].UserCharacter;
            if(MyID.Equals(userinfo.playerInfo.USER[i].UserID))
            {
                PlayerisImposter = userinfo.playerInfo.USER[i].isImposter;
                imageNumber = i;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
