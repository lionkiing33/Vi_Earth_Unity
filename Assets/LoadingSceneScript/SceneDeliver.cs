using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDeliver : MonoBehaviour
{
    public static SceneDeliver sceneDeliver;
    public UserInfo playerInfo = new UserInfo();
    public string userID;
    private void Awake()
    {
        sceneDeliver = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
}
