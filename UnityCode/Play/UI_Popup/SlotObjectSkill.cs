using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotObjectSkill : MonoBehaviour
{
    public CharacterSkill characterSkill;

    public void UseSkill()
    {
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();

        if (!controller.UseSkill(characterSkill.SlotNum))
            return;

        Debug.Log(gameObject.name + characterSkill.SlotNum + "스킬 사용" + characterSkill.Mp);
    }
}
