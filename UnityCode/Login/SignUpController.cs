using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpController : MonoBehaviour {

    public InputField InputID;
    public InputField InputPW;

    public GameObject LoginPopup;
    public GameObject InfomationPopup;

    void Cancel()
    {
        LoginPopup.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void SignUp()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("ID", InputID.text);
        data.Add("PW", InputPW.text);

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForSignUp;
        httpHelper.post(100, "/SignUp", data);
    }

    void OnHttpRequestForSignUp(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
            this.InfomationPopup.SetActive(true);
            InfomationPopup info = this.InfomationPopup.GetComponent<InfomationPopup>();
            info.Title.text = "Sign Up";
            info.Description.text = "아이디를 확인하세요";

        }
        else
        {
            // >> : 1. 회원가입에 성공하였습니다. 팝업띄우기 
            //    : 2. 회원가입창 닫고, 로그인창 띄우기

            this.InfomationPopup.SetActive(true);
            InfomationPopup info = this.InfomationPopup.GetComponent<InfomationPopup>();
            info.Title.text = "Sign Up";
            info.Description.text = "회원가입에 성공하였습니다.";

            Cancel();
            // << :
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForSignUp;
    }
}
