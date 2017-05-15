using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 tempVec;

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempVec = new Vector2( gameObject.transform.position.x, gameObject.transform.position.y) - eventData.position;
        Debug.Log(eventData.position);
        Debug.Log(gameObject.transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position + tempVec;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

}
