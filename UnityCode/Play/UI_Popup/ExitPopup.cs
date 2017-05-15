using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : MonoBehaviour
{
    void OK()
    {
        PlaySceneController pc = GameObject.Find("Controller").GetComponent<PlaySceneController>();
        pc.Exit();
        this.gameObject.SetActive(false);

        SceneManager.LoadScene("LoginScene");
    }

    void Cancel()
    {
        // 팝업 닫아준다.
        this.gameObject.SetActive(false);
    }
}
