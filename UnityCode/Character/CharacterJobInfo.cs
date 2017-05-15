using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJobInfo
{
    public int JobId;
    public string Name;
    public string PrefabName;
    public string Description;

    public List<CharacterSkill> CharacterSkillList = new List<CharacterSkill>();

    public CharacterJobInfo()
    {

    }

    public CharacterJobInfo(int _id, string _name, string _prefabName, string _description)
    {
        JobId = _id;
        Name = _name;
        PrefabName = _prefabName;
        Description = _description;
    }

    public CharacterJobInfo(int _id, string _prefabName)
    {
        JobId = _id;
        PrefabName = _prefabName;
    }

    public void AddSkill(CharacterSkill skill)
    {
        CharacterSkillList.Add(skill);
    }
}
