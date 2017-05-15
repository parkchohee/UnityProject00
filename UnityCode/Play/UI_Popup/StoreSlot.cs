using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    Item ItemInfo;

    public Image ItemImage;
    public Text ItemName;
    public Text ItemPrice;

    public GameObject CountPopup;

    public void StoreSlotSetting(Item _ItemInfo)
    {
        ItemInfo = _ItemInfo;

        Sprite newSprite = Resources.Load<Sprite>("Images/Items/" + _ItemInfo.PrefabName);
        this.ItemImage.sprite = newSprite;
        ItemName.text = _ItemInfo.Name;
        ItemPrice.text = _ItemInfo.Price.ToString();
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    void BuyBtn()
    {
        CountPopup.SetActive(true);
        CountPopup.GetComponent<StoreCountPopup>().itemID = ItemInfo.ItemID;
    }
}
