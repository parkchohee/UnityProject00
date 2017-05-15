using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotKeySlot : Slot
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (item)
            Destroy(item);

        GameObject DragItem = Instantiate<GameObject>(DragHandler.item, this.gameObject.transform.position, this.gameObject.transform.rotation);
        DragItem.transform.SetParent(transform);


        if (DragItem.GetComponent<SlotObject>().slotType == SlotObject.SLOT_TYPE.ITEM_SLOT)
        {
            DragItem.GetComponent<SlotObjectItem>().item = DragHandler.item.GetComponent<SlotObjectItem>().item;

            RectTransform rt = gameObject.GetComponent<RectTransform>();
            DragItem.GetComponent<GridLayoutGroup>().cellSize = new Vector2(rt.rect.width - 1, rt.rect.height - 1);
        }
        else if(DragItem.GetComponent<SlotObject>().slotType == SlotObject.SLOT_TYPE.SKILL_SLOT)
        {
            DragItem.GetComponent<SlotObjectSkill>().characterSkill = DragHandler.item.GetComponent<SlotObjectSkill>().characterSkill;
        }


        DragItem.GetComponent<CanvasGroup>().blocksRaycasts = true;

        // >> : 단축키 슬롯에서 드래그하면 이동으로 처리한다.
        if (DragHandler.item.transform.parent.gameObject.tag == "HotKeySlot")
        {
            Destroy(DragHandler.item);
        }
    }
}
