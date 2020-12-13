using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Bell : MonoBehaviour
{
    public GameObject player;
    public GameObject emergency_meeting;
    private GameObject bell;
    private AudioSource audioSource;
    private int isClicked;
    private bool isItClear = false;
    private Bell bell_script;

    // Start is called before the first frame update
    void OnEnable()
    {
        bell = this.transform.parent.transform.parent.gameObject;
        audioSource = this.GetComponent<AudioSource>();
        bell_script = this.GetComponent<Bell>();
        isClicked = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked == 3 && player != null && isItClear)
        {
            PlayerMeeting playerMeeting = new PlayerMeeting();
            playerMeeting.isItMeeting = true;
            playerMeeting.playerName = player.name;

            string data = JsonUtility.ToJson(playerMeeting, prettyPrint: true);
            SocketManger.Socket.Emit("SendPlayerMeeting", data);

            emergency_meeting.SetActive(true);
            isItClear = false;

            bell_script.enabled = false;

            bell.SetActive(false);
        }
    }

    public void Ring_The_Bell()
    {
        isClicked += 1;
        audioSource.Play();
        if(isClicked == 3)
        {
            isItClear = true;
        }
    }
}

public class PlayerMeeting
{
    public bool isItMeeting;
    public string playerName;
}
