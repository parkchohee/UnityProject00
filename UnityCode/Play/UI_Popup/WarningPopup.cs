using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPopup : MonoBehaviour {

    public Text text;

    void OK()
    {
        this.gameObject.SetActive(false);
    }
}
