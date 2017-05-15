using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour {

    public GameObject InventorySlotObject = null;
    public GameObject InventoryContentPanel = null;
    public Text moneyText;

    private int MaxInventorySlot = 18;
    private InventorySlot[] InventorySlotList;
    
    public void Setting(List<Item> itemList) {
        InventorySlotList = new InventorySlot[MaxInventorySlot];

        for (int i = 0; i < MaxInventorySlot; i++)
        {
            GameObject obj = Instantiate(InventorySlotObject, Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(InventoryContentPanel.gameObject.transform);

            InventorySlot in_item = obj.GetComponent<InventorySlot>();
            in_item.SettingEnabled(false);

            InventorySlotList[i] = in_item;
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            InventorySlot in_item = InventorySlotList[itemList[i].ItemSlotNum];
            in_item.SlotSetting(itemList[i]);
        }
    }
	
    public int AddItem(Item item, int count)
    {
        for (int i = 0; i< InventorySlotList.Length; i++)
        {
            if (InventorySlotList[i].item == null)
                continue;

            if(InventorySlotList[i].item.ItemID == item.ItemID)
            {
                InventorySlotList[i].item.ItemCount += count;
                InventorySlotList[i].SlotSetting(InventorySlotList[i].item);
                return i;
            }
        }

        for (int i = 0; i< InventorySlotList.Length; i++)
        {
            if (InventorySlotList[i].item != null)
                continue;

            InventorySlotList[i].item = item;
            InventorySlotList[i].item.ItemCount = count;
            InventorySlotList[i].item.ItemSlotNum = i;
            InventorySlotList[i].SlotSetting(item);
            return i;
        }

        return -1;
    }

    public void UseItem(int slotNum)
    {
        InventorySlot slot = InventorySlotList[slotNum];
        slot.SlotSetting(slot.item);
    }

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
