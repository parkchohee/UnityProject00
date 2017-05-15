using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotObjectItem : MonoBehaviour
{
    public Item item;
 
    public void UseItem()
    {
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();

        if (!controller.UseItem(item.ItemID))
            return;
        
        gameObject.GetComponentInChildren<Text>().text = item.ItemCount.ToString();
    }
}
