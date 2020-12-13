using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyMeeting : MonoBehaviour
{
    public GameObject myPlayer;
    public string meetingId;
    public string[] allPlayerName = new string[5];
    public string[] allPlayerImageName = new string[5];

    public GameObject discuss;
    private Discuss discuss_script;
    private EmergencyMeeting emergency;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void OnEnable()
    {
        emergency = this.GetComponent<EmergencyMeeting>();
        coroutine = Show_Image(3);
        StartCoroutine(coroutine);
    }

    IEnumerator Show_Image(int time)
    {
        while (true) //무한 반복
        {
            Debug.Log(time);
            if (time == 0)
            {
                discuss.SetActive(true); 
                discuss.transform.GetChild(0).GetComponent<Discuss>().playerTextString = allPlayerName;
                discuss.transform.GetChild(0).GetComponent<Discuss>().playerImageString = allPlayerImageName;
                discuss.transform.GetChild(0).GetComponent<Discuss>().playerMeetingId = meetingId;
                discuss.transform.GetChild(0).GetComponent<Discuss>().player = myPlayer;
                emergency.enabled = false;
                this.gameObject.SetActive(false);
                break;
            }
            else if (time != 0)
            {
                time--; //1씩 감소
                yield return new WaitForSeconds(1f); //1초 딜레이
            }
        }
    }
}