using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour {

    public InputField InputID;
    public InputField InputPW;

    public GameObject SignUpPopup;
    public GameObject InfomationPopup;

    void Login()
    {
        GameManager.Instance.LoginId = InputID.text;

        Debug.Log(InputID.text);
        Debug.Log(InputPW.text);

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("ID", InputID.text);
        data.Add("PW", InputPW.text);

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForLogin;
        httpHelper.post(100, "/Login", data);
    }

    void SignUpBtn()
    {
        this.gameObject.SetActive(false);
        SignUpPopup.gameObject.SetActive(true);
    }

    void OnHttpRequestForLogin(int id, WWW www)
    {
        if (www.error != null)
        {
            this.InfomationPopup.SetActive(true);
            InfomationPopup info = this.InfomationPopup.GetComponent<InfomationPopup>();
            info.Title.text = "Login";

            Debug.Log(www.text);
            if (www.text == "\"WRONG_PASSWORD\"")
            {
                info.Description.text = "비밀번호를 확인하세요.";

            }
            else
            {
                info.Description.text = "등록되지 않은 아이디 입니다.";
            }
        }
        else
        {
            GameManager.Instance.NextSceneName = "SlotSelectScene";
            SceneManager.LoadScene("LoadingScene");
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForLogin;
    }

}
