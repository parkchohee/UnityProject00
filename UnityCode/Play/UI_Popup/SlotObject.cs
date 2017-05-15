using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotObject : MonoBehaviour {
    public enum SLOT_TYPE
    {
        ITEM_SLOT,
        SKILL_SLOT
    }

    public SLOT_TYPE slotType;


    public virtual void UseSlotObject()
    {
        if (slotType == SLOT_TYPE.ITEM_SLOT)
        {
            Debug.Log("item");
            SlotObjectItem obj = gameObject.GetComponent<SlotObjectItem>();
            obj.UseItem();

        }
        else if (slotType == SLOT_TYPE.SKILL_SLOT)
        {
            Debug.Log("skill");
            SlotObjectSkill obj = gameObject.GetComponent<SlotObjectSkill>();
            obj.UseSkill();
        }
    }
}
