using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System;

public class BtnType : MonoBehaviour
{
    public BTNType currentType;
    public CanvasGroup StartMenu, LoginMenu, SignUpMenu, FindMenu, MainMenu, PlayMenu, TutorialMenu, CashShopMenu, JoinMenu;
    public InputField ID_text, Password_text, Email_text, Name_text;
    public static string ipget= ip.IP;

    public static Boolean IDcheck = false;


    public void Start()
    {
        CanvasGroupOn(StartMenu);
        CanvasGroupOff(PlayMenu);
        CanvasGroupOff(LoginMenu);
        CanvasGroupOff(SignUpMenu);
        CanvasGroupOff(FindMenu);
        CanvasGroupOff(MainMenu);
        CanvasGroupOff(TutorialMenu);
        CanvasGroupOff(CashShopMenu);
        CanvasGroupOff(JoinMenu);
    }
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Login:
                CanvasGroupOn(LoginMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.SignUp:
                CanvasGroupOn(SignUpMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.Pl_Login:
                if (string.IsNullOrEmpty(ID_text.text) == false && string.IsNullOrEmpty(Password_text.text) == false)
                {
                    StartCoroutine(loginWithWWW());
                }
                else
                {
                    Toast.Instance.Show("ID와 PassWord를 입력해주세요", 3f);
                }
                break;
            case BTNType.Pl_SignUp:
                if(string.IsNullOrEmpty(ID_text.text) == false)
                {
                    if(IDcheck)
                    {
                        if(string.IsNullOrEmpty(Password_text.text) == false && string.IsNullOrEmpty(Email_text.text) == false)
                        {
                            StartCoroutine(PostNetworkingWithWWW());
                            CanvasGroupOn(StartMenu);
                            CanvasGroupOff(LoginMenu);
                            CanvasGroupOff(SignUpMenu);
                            CanvasGroupOff(FindMenu);
                            ID_text.text = "";
                            Password_text.text = "";
                            Email_text.text = "";
                            Debug.Log("창을 닫아요");
                        }else
                        {
                            Toast.Instance.Show("E-mail과 PW를 입력해주세요", 3f);
                        }
                    }else
                    {
                        Toast.Instance.Show("ID중복체크를 해주세요.", 3f);
                    }
                }else
                {
                    Toast.Instance.Show("ID를 입력해주세요", 3f);
                }
                break;
            case BTNType.Cancel:
                CanvasGroupOn(StartMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.Find:
                CanvasGroupOn(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.Find_ID:
                if (string.IsNullOrEmpty(Email_text.text) == false)
                {
                    StartCoroutine(FindIDWithWWW());
                }
                else
                {
                    Toast.Instance.Show("Email을 입력해주세요", 3f);
                }
                break;
            case BTNType.Find_PW:
                if (string.IsNullOrEmpty(ID_text.text) == false && string.IsNullOrEmpty(Email_text.text) == false)
                {
                    StartCoroutine(FindPWWithWWW());
                }
                else
                {
                    Toast.Instance.Show("ID와 Email을 입력해주세요", 3f);
                }
                break;
            case BTNType.IDcheck:
                if (string.IsNullOrEmpty(ID_text.text) == false)
                {
                    StartCoroutine(idcheckWithWWW());
                }else
                {
                    Toast.Instance.Show("ID를 입력해주세요",3f);
                }
                break;
            case BTNType.Play:
                if (string.IsNullOrEmpty(Name_text.text) == false)
                {
                    StartCoroutine(UpdateNameWithWWW());
                }else
                {
                    Toast.Instance.Show("닉네임을 설정해주세요");
                }
                    break;
            case BTNType.Tutorial:
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOn(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.CashShop:
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOn(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.Logout:
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOn(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
            case BTNType.Join:
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOn(JoinMenu);
                break;
            case BTNType.Back:
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOn(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
                break;
        }
    }
    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    IEnumerator PostNetworkingWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);
        form.AddField("password", Password_text.text);
        form.AddField("email", Email_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://"+ipget+"process/adduser", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("회원가입에 실패했습니다.", 3f);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Toast.Instance.Show("회원가입이 성공했습니다.", 3f);
        }
    }
    public class res_idcheck
    {
        public string message;
    }
    IEnumerator idcheckWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://" + ipget + "process/idcheck", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("서버 접속에 실패했습니다.", 3f);
        }else
        {
            Debug.Log(www.downloadHandler.text);
            var res_idcheck = JsonConvert.DeserializeObject<res_idcheck>(www.downloadHandler.text);
            Debug.LogFormat("{0}", res_idcheck.message);
            if (res_idcheck.message.Equals("yes")) {
                Toast.Instance.Show("사용가능한 ID입니다.", 3f);
                IDcheck = true;
            }else
            {
                Toast.Instance.Show("중복된 ID입니다. 다른 ID를 입력해주세요", 3f);
            }
        }
    }
    public class res_findid
    {
        public string id;
    }
    IEnumerator FindIDWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", Email_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://" +ipget + "process/findid", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("네트워크 연결에 실패했습니다", 3f);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text.Substring(12,4).Equals("NULL"))
            {
                Toast.Instance.Show("ID가 없습니다.", 3f);
            }
            else
            {
                Toast.Instance.Show(www.downloadHandler.text.Substring(11), 3f);
            }
        }
    }
    public class res_findpw
    {
        public string pw;
    }
    IEnumerator FindPWWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);
        form.AddField("email", Email_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://" + ipget + "process/findpw", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("네트워크 연결에 실패했습니다", 3f);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var res_findpw = JsonConvert.DeserializeObject<res_findpw>(www.downloadHandler.text);
            Debug.LogFormat("{0}", res_findpw.pw);
            Toast.Instance.Show(res_findpw.pw, 3f);
        }
    }
    public class res_login
    {
        public string message;
    }
    IEnumerator loginWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);
        form.AddField("password", Password_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://" + ipget + "process/login", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("서버 접속에 실패했습니다.", 3f);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var res_login = JsonConvert.DeserializeObject<res_login>(www.downloadHandler.text);
            Debug.LogFormat("{0}", res_login.message);
            if (res_login.message.Equals("로그인 성공"))
            {
                Toast.Instance.Show("Login Sucess.", 3f);
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOn(MainMenu);
                CanvasGroupOff(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
            }
            else
            {
                Toast.Instance.Show("ID와 PW가 다릅니다.", 3f);
            }
        }
    }
    public class res_name
    {
        public string message;
    }
    IEnumerator UpdateNameWithWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", ID_text.text);
        form.AddField("name", Name_text.text);

        UnityWebRequest www = UnityWebRequest.Post("http://" + ipget + "process/nickname", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Toast.Instance.Show("서버 접속에 실패했습니다.", 3f);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var res_name = JsonConvert.DeserializeObject<res_name>(www.downloadHandler.text);
            Debug.LogFormat("{0}", res_name.message);
            if (res_name.message.Equals("닉네임 설정성공"))
            {
                Toast.Instance.Show("닉네임 설정 성공.", 3f);
                CanvasGroupOff(FindMenu);
                CanvasGroupOff(LoginMenu);
                CanvasGroupOff(SignUpMenu);
                CanvasGroupOff(StartMenu);
                CanvasGroupOff(MainMenu);
                CanvasGroupOn(PlayMenu);
                CanvasGroupOff(TutorialMenu);
                CanvasGroupOff(CashShopMenu);
                CanvasGroupOff(JoinMenu);
            }
            else
            {
                Toast.Instance.Show("닉네임 설정 실패.", 3f);
            }
        }
    }
}