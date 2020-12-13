using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject fadePanel;
    private Image fade;
    private float fadeIn = 0.5f;
    private float fadeOut = 0.0f;
    private int time = 60;
    public IEnumerator coroutine;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        fadePanel.SetActive(false);
        audioSource = this.GetComponent<AudioSource>();
        coroutine = FadeInAndOut(time);
        fade = fadePanel.GetComponent<Image>();
        fade.color = new Color(255, 0, 0, fadeOut);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FadeInAndOut(int time)
    {
        fadePanel.SetActive(true);
        while (true) //무한 반복
        {
            //종료
            if (time == 0)
            {
                fadePanel.SetActive(false);
                audioSource.Stop();
                break;
            }
            //투표 진행중
            else
            {
                if (time % 2 == 0)
                {
                    audioSource.Play();
                    fade.color = new Color(255, 0, 0, fadeIn);
                }
                else
                {
                    audioSource.Stop();
                    fade.color = new Color(255, 0, 0, fadeOut);
                }
                time--; //1씩 감소
                yield return new WaitForSeconds(1f); //1초 딜레이
            }
        }
    }
}
