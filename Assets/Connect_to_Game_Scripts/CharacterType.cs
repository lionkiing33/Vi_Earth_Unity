using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CharacterType : MonoBehaviour
{
    public InputField ID_text;
    public Toggle Character;
    public static string ipget = ip.IP;
    public static string character_name;
    public int c_character_number;

    private void Awake()
    {
        Character.onValueChanged.AddListener((On) => //토글의 값이 변하면
        {
            if(On)//토글이 체크되면
            {
                character_name = Character.GetComponentInChildren<Text>().text;//토글의 케릭터이름 저장
                Debug.Log(character_name);
            }
        });
    }
    public void BuyBtnClick()
    {
        Debug.Log("Buy");
    }
    public void SelectBtnClick()
    {
        StartCoroutine(UpdateCharacterWithWWW());
        switch(character_name)
        {
            case "Man":
                c_character_number = 0;
                break;
            case "Woman":
                c_character_number = 1;
                break;
            case "Dracula":
                c_character_number = 2;
                break;
            case "Franken":
                c_character_number = 3;
                break;
            case "KangSi":
                c_character_number = 4;
                break;
            case "Mummy":
                c_character_number = 5;
                break;
            case "Witch":
                c_character_number = 6;
                break;
        }
    }
    public class res_character
    {
        public string message;
    }
    IEnumerator UpdateCharacterWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);
        form.AddField("character", character_name);

        UnityWebRequest www = UnityWebRequest.Post("http://" + ipget + "process/updatecharacter", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("서버 접속에 실패했습니다.", 3f);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var res_character = JsonConvert.DeserializeObject<res_character>(www.downloadHandler.text);
            Debug.LogFormat("{0}", res_character.message);
            if (res_character.message.Equals("캐릭터 설정 성공"))
            {
                Toast.Instance.Show("캐릭터를" + character_name + "로 설정했습니다.", 3f);
            }
            else
            {
                Toast.Instance.Show("캐릭터 설정 실패.", 3f);
            }
        }
    }
}