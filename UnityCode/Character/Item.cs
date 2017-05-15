using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{


    public enum ITEM_TYPE
    {
        HEALTH_ITEM,
        MANA_ITEM
    }

    public ITEM_TYPE ItemType;



    public int ItemID;
    public string Name;
    public string PrefabName;
    public int Price;
   // public int ItemType;
    public int ItemPower;
    public int ItemCount;
    public int ItemSlotNum;


    public Item(int _itemID, string _name, string _prefabName, int _price, int _itemType, int _itemPower, int _ItemCount = 1, int _ItemSlotNum = 0)
    {
        ItemID = _itemID;
        Name = _name;
        PrefabName = _prefabName;
        Price = _price;
        ItemType = (Item.ITEM_TYPE)_itemType;
        ItemPower = _itemPower;
        ItemCount = _ItemCount;
        ItemSlotNum = _ItemSlotNum;
    }
}
