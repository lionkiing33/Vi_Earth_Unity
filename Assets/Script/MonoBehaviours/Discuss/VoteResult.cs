using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteResult : MonoBehaviour
{
	public GameObject joystick; 
    public Bell bell_script;
    public EmergencyMeeting emergency_script;
    public Discuss discuss_script;
    public ChatManager chat_script;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void OnEnable()
    {
        coroutine = Show_Image(5);
        StartCoroutine(coroutine);
    }

    IEnumerator Show_Image(int time)
    {
        bell_script.enabled = true;
        emergency_script.enabled = true;
        discuss_script.enabled = false;
        discuss_script.enabled = true;
        chat_script.enabled = false;
        chat_script.enabled = true;

        while (true) //무한 반복
        {
            if (time == 0)
            {
				joystick.SetActive(true);
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
