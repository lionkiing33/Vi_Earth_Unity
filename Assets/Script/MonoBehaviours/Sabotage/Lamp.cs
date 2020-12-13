using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    private GameObject lamp;
    private const int numOfSwitch = 4;
    private Image[] switchImage = new Image[numOfSwitch];
    public const int numOfState = 2;
    public Sprite[] switchState = new Sprite[numOfState];
    private int length;
    private int[] index;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        lamp = this.transform.parent.gameObject;
        for (int i = 0; i < numOfSwitch; i++)
        {
            switchImage[i] = this.transform.GetChild(0).transform.GetChild(i).gameObject.GetComponent<Image>();
            switchImage[i].sprite = switchState[0];
        }
        length = Random.Range(1, 5);
        Debug.Log(length);
        index = new int[length];
        for (int i = 0; i < length; i++)
        {
            index[i] = Random.Range(0, 4);
            for (int j = 0; j < i;)
            {
                if (index[i] == index[j])
                {
                    index[i] = Random.Range(0, 4);
                }
                else
                {
                    j++;
                }
            }
            Debug.Log(index[i]);
            switchImage[index[i]].sprite = switchState[1];
        }
        audioSource = this.GetComponent<AudioSource>();
    }

    public void Switch_On(int index)
    {
        audioSource.Play();
        if (switchImage[index].sprite == switchState[1])
        {
            switchImage[index].sprite = switchState[0];
        }
    }

    public void Switch_Off(int index)
    {
        audioSource.Play();
        if (switchImage[index].sprite == switchState[0])
        {
            switchImage[index].sprite = switchState[1];
        }
    }

    public void Exit_Panel()
    {
        lamp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(switchImage[0].sprite == switchState[0] && switchImage[1].sprite == switchState[0] && switchImage[2].sprite == switchState[0] && switchImage[3].sprite == switchState[0])
        {
            Exit_Panel();
        }
    }
}
