using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySceneUIController : MonoBehaviour
{
    public GameObject SkillPop;
    public GameObject InventoryPop;
    public GameObject StorePop;
    public GameObject StoreCountPop;
    public GameObject PlayerInfoSlot;
    public GameObject ExitPop;
    public GameObject DiePop;
    public GameObject WarningPop;

    bool isPause = false;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            if (isPause)
            {
                ExitPop.SetActive(true);
            }
            else
            {
                ExitPop.SetActive(false);
            }
        }

    }

    public void SkillUISetting(List<CharacterSkill> skillList)
    {
        SkillPopup pop = SkillPop.GetComponent<SkillPopup>();
        pop.Setting(skillList);
    }

    public void InventoryUISetting(List<Item> itemList)
    {
        InventoryPopup inventory = InventoryPop.GetComponent<InventoryPopup>();
        inventory.Setting(itemList);
    }

    public void StoreUISetting(List<Item> itemList)
    {
        StorePopup store = StorePop.GetComponent<StorePopup>();
        store.Setting(itemList, StoreCountPop);
    }

    void InventoryOpen()
    {
        InventoryPop.SetActive(true);
    }
    
    void SkillOpen()
    {
        SkillPop.SetActive(true);
    }

    public void InventoryUseUpdate(int slotNum)
    {
        InventoryPopup inventory = InventoryPop.GetComponent<InventoryPopup>();
        inventory.UseItem(slotNum);
    }

    public void InventoryMoneySetting(int money)
    {
        InventoryPopup inventory = InventoryPop.GetComponent<InventoryPopup>();
        inventory.SetMoney(money);
    }

    public void SkillLevelSetting(int skillNum, int skillLevel)
    {
        SkillPopup pop = SkillPop.GetComponent<SkillPopup>();
        pop.SetSkillLevel(skillNum, skillLevel);
    }

    public void SkillPointSetting(int skillPoint)
    {
        SkillPopup pop = SkillPop.GetComponent<SkillPopup>();
        pop.SetSkillPoint(skillPoint);
    }

    public int InventoryAdd(Item item, int count)
    {
        InventoryPopup inventory = InventoryPop.GetComponent<InventoryPopup>();
        return inventory.AddItem(item, count);
    }

    public void PlayerInfoSlotSetting(string prefabName, int level)
    {
        PlayerInfoSlot slot = PlayerInfoSlot.GetComponent<PlayerInfoSlot>();
        Sprite newSprite = Resources.Load<Sprite>("Images/UI/" + prefabName);
        slot.playerJobImage.sprite = newSprite;
        slot.PlayerLevel.text = level.ToString();
    }
   
    public void Warning(string text)
    {
        WarningPop.SetActive(true);
        WarningPop.GetComponent<WarningPopup>().text.text = text;
    }
}
