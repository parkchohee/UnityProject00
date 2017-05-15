using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPopup : MonoBehaviour {

    public GameObject[] SkillSlots;
    public Text SkillPointText;
    
    public void Setting(List<CharacterSkill> skillList)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            SkillSlot slot = SkillSlots[i].GetComponent<SkillSlot>();
            slot.SettingSkill(skillList[i], skillList[i].SlotNum);
            //slot.SettingSkill(skillList[i].ImageName, skillList[i].SkillLevel);
        }
    }

    void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }

    public void SetSkillLevel(int skillNum, int skillLevel)
    {
        SkillSlots[skillNum].GetComponent<SkillSlot>().SetSkillLevel(skillLevel);
    }

    public void SetSkillPoint(int _skillPoint)
    {
        SkillPointText.text = _skillPoint.ToString();
    }
}
