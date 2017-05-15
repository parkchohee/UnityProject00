using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill 
{
    public enum SKILL_TYPE
    {
        ATTACK_SKILL,
        HEALTH_SKILL
    }

    public SKILL_TYPE skillTypeEnum;

    public int Id;
    public string Name;
    public string Description;
    public string ImageName;
    public int SkillType;
    public int SkillPower;
    public int JobId;

    public int SlotNum;
    public int Mp;


    public int SkillLevel = 1; 

    public CharacterSkill(int _Id, string _Name, string _Description, string _ImageName, int _SkillType, int _SkillPower, int _JobId, int _SlotNum, int _Mp)
    {
        Id = _Id;
        Name = _Name;
        Description = _Description;
        ImageName = _ImageName;
        SkillType = _SkillType;
        SkillPower = _SkillPower;
        JobId = _JobId;
        SlotNum = _SlotNum;
        Mp = _Mp;

        skillTypeEnum = (CharacterSkill.SKILL_TYPE)_SkillType;
    }

    public CharacterSkill(int _Id, string _Name, string _ParticleName, string _ImageName, int _SkillType, int _SkillPower, int _JobId, int _SlotNum, int _Mp, int _skillLevel)
        : this(_Id, _Name, _ParticleName, _ImageName, _SkillType, _SkillPower, _JobId, _SlotNum, _Mp)
    {
        SkillLevel = _skillLevel;
    }
}
