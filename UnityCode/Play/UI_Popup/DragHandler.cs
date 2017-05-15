using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject item;

    Transform startParent;
    Vector3 startPosition;


    GameObject tempItem = null;



    public void OnBeginDrag(PointerEventData eventData)
    {
        item = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        tempItem = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Transform tempParent = GameObject.Find("TopPanel").transform;
        tempItem.transform.SetParent(tempParent);

        tempItem.GetComponent<RectTransform>().sizeDelta = new Vector2(40,40);
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        tempItem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item = null;

        if (tempItem != null)
            Destroy(tempItem);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }

}