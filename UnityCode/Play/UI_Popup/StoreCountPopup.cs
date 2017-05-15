using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCountPopup : MonoBehaviour {

    public InputField inputCount;
    public int itemID;
    
    void Close()
    {
        this.gameObject.SetActive(false);
    }

    void Buy()
    {
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();

        controller.BuyItem(itemID, int.Parse(inputCount.text));
        inputCount.text = null;
        gameObject.SetActive(false);
    }
}
