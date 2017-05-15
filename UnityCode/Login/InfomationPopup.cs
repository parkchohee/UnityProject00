using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfomationPopup : MonoBehaviour {

    public Text Title;
    public Text Description;
    
    void CloseBtn()
    {
        this.gameObject.SetActive(false);
    }
}
