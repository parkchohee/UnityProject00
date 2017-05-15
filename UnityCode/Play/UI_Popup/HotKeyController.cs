using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyController : MonoBehaviour {

    public Slot HotKeySlotQ;
    public Slot HotKeySlotW;
    public Slot HotKeySlotE;
    public Slot HotKeySlotA;
    public Slot HotKeySlotS;
    public Slot HotKeySlotD;

    public Slot HotKeySlotInsert;
    public Slot HotKeySlotHome;
    public Slot HotKeySlotPageUp;
    public Slot HotKeySlotDelete;
    public Slot HotKeySlotEnd;
    public Slot HotKeySlotPageDown;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            SlotObject obj = null;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                obj = HotKeySlotQ.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                obj = HotKeySlotW.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                obj = HotKeySlotE.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                obj = HotKeySlotA.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                obj = HotKeySlotS.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                obj = HotKeySlotD.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.Insert))
            {
                obj = HotKeySlotInsert.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.Home))
            {
                obj = HotKeySlotHome.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.PageUp))
            {
                obj = HotKeySlotPageUp.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                obj = HotKeySlotDelete.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.End))
            {
                obj = HotKeySlotEnd.GetComponentInChildren<SlotObject>();
            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                obj = HotKeySlotPageDown.GetComponentInChildren<SlotObject>();
            }

            if (obj == null)
                return;
            obj.UseSlotObject();
        }
    }
}
