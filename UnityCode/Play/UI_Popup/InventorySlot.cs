using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // >> :
    public Item item;
    // << :

    //public int ItemID;
    //public int ItemCount;

    public Image ItemImage;
    public Text ItemCountText;


    public void SlotSetting(Item _item)
    {
        SettingEnabled(true);

        item = _item;

        Sprite newSprite = Resources.Load<Sprite>("Images/Items/" + _item.PrefabName);
        
        this.ItemImage.sprite = newSprite;
        ItemCountText.text = _item.ItemCount.ToString();

        SlotObjectItem slotObjectItem = ItemImage.gameObject.GetComponent<SlotObjectItem>();
        slotObjectItem.item = _item;
    }



    //public void SlotSetting(int id, string name, int count, int power, int itemType)
    //{
    //    SettingEnabled(true);

    //    ItemID = id;
    //    ItemCount = count;

    //    ItemImage.sprite = Resources.Load<Sprite>(name);
    //    ItemCountText.text = count.ToString();

    //    SlotObjectItem item = ItemImage.gameObject.GetComponent<SlotObjectItem>();
    //    item.itemType = (SlotObjectItem.ITEM_TYPE)itemType;
    //    item.itemPower = power;
    //    item.itemID = id;
    //}

    public void SettingEnabled(bool isEnabled)
    {
        ItemImage.enabled = isEnabled;
        ItemCountText.enabled = isEnabled;
    }

    public void UseItem()
    {
        Debug.Log(item.ItemCount);
        item.ItemCount--;
        ItemCountText.text = item.ItemCount.ToString();
    }
}
