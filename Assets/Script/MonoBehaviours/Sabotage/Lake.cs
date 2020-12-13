using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Lake : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Text information;
    private GameObject scannerLight;
    private Image scannerLightImage;
    private Vector3 scannerLightPosition;
    private Vector3 fingerPrintPosition;
    private Color normal = new Color(255,255,255,255);
    private Color transparent = new Color(255, 255, 255, 0);
    private string scannerState;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        information = this.transform.parent.transform.parent.transform.parent.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.transform.GetComponent<Text>();
        information.text = "길게 눌러 호수 안정화";

        scannerLight = this.transform.parent.gameObject;
        scannerLightImage = scannerLight.GetComponent<Image>();
        scannerLightImage.color = transparent;

        scannerLightPosition = scannerLight.transform.localPosition;
        fingerPrintPosition = this.transform.localPosition;

        scannerState = "Going Up";
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(scannerState)
        {
            case "Going Up":
                increase_scanner_light_position();
                break;
            case "Going Down":
                decrease_scanner_light_position();
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        information.text = "다른 유저 기다리는 중..";
        scannerLightImage.color = normal;
        audioSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        information.text = "길게 눌러 호수 안정화";
        scannerLightImage.color = transparent;
    }

    public void increase_scanner_light_position()
    {
        if(scannerLightPosition.y >= 265.0f)
        {
            scannerState = "Going Down";
        }
        else
        {
            scannerLightPosition.y += 1.0f;
            fingerPrintPosition.y -= 1.0f;
            scannerLight.transform.localPosition = scannerLightPosition;
            this.transform.localPosition = fingerPrintPosition;
        }
    }

    public void decrease_scanner_light_position()
    {
        if (scannerLightPosition.y <= -265.0f)
        {
            scannerState = "Going Up";
        }
        else
        {
            scannerLightPosition.y -= 1.0f;
            fingerPrintPosition.y += 1.0f;
            scannerLight.transform.localPosition = scannerLightPosition;
            this.transform.localPosition = fingerPrintPosition;
        }
    }
}
