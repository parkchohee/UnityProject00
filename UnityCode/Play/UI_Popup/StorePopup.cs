using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePopup : MonoBehaviour
{
    public GameObject StoreSlotObject;
    public GameObject StoreContentsPanel;
    List<StoreSlot> StoreSlotList;

    public void Setting(List<Item> ItemList, GameObject countPop)
    {
        StoreSlotList = new List<StoreSlot>();

        for (int i = 0; i < ItemList.Count; i++)
        {
            // storeslot을 생성하고,
            GameObject storeSlotObj = Instantiate(StoreSlotObject, Vector3.zero, Quaternion.identity) as GameObject;
            storeSlotObj.transform.SetParent(StoreContentsPanel.transform);

            // 데이터 셋팅
            StoreSlot storeSlot = storeSlotObj.GetComponent<StoreSlot>();
            storeSlot.StoreSlotSetting(ItemList[i]);
            storeSlot.CountPopup = countPop;

            // storeslotlist에추가
            StoreSlotList.Add(storeSlot);
        }
    }

    void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
