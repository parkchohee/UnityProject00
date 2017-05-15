using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    CharacterSkill characterSkill;

    public Image SkillImageBackground;
    public Image SkillImageForeground;
    public Text SkillLevelText;
    public int SkillLevel;

    int slotNum;

    void SkillLevelUp()
    {
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();
        controller.SkillLevelUp(slotNum);
    }

    void SkillLevelDown()
    {
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();
        controller.SkillLevelDown(slotNum);
    }

    public void SettingSkill(CharacterSkill _characterSkill, int _slotNum)
    {
        characterSkill = _characterSkill;

        Sprite newSprite = Resources.Load<Sprite>("Images/Skills/" + _characterSkill.ImageName);
        this.SkillImageBackground.sprite = newSprite;
        this.SkillImageForeground.sprite = newSprite;

        SetSkillLevel(_characterSkill.SkillLevel);

        slotNum = _slotNum;

        SlotObjectSkill slotObjectSkill = gameObject.GetComponentInChildren<SlotObjectSkill>();
        slotObjectSkill.characterSkill = _characterSkill;
    }

    public void SetSkillLevel(int level)
    {
        this.SkillLevel = level;
        SkillLevelText.text = SkillLevel.ToString();
    }
}
