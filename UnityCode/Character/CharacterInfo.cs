using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public string userId;
    public int SlotNum;
    public int CharacterNum;
    public string CharacterName;
    public int JobId;
    public int CurrentExp;
    public int MaxExp;
    public int CurrentMp;
    public int MaxMp;
    public int CurrentHp;
    public int MaxHp;
    public int[] skillLev;

    public int SkillPoint;
    public int Money;
    public int AttackPower;
    public int Level;

    public Vector3 SpawnPoint;

    public CharacterJobInfo JobInfo;

    public CharacterInfo()
    {
        skillLev = new int[3];
        JobInfo = new CharacterJobInfo();
    }

    public CharacterInfo(string _userId, int _slotNum, int _characterNum, string _characterName,
        int _JobId, int _currentExp, int _maxExp, int _currentMp, int _maxMp, int _cureentHp, int _maxHp, int _level,
        int _skillLev1, int _skillLev2, int _skillLev3)
    {
        userId = _userId;
        SlotNum = _slotNum;
        CharacterNum = _characterNum;
        CharacterName = _characterName;
        JobId = _JobId;

        CurrentExp = _currentExp;
        MaxExp = _maxExp;
        CurrentMp = _currentMp;
        MaxMp = _maxMp;
        CurrentHp = _cureentHp;
        MaxHp = _maxHp;
        Level = _level;

        skillLev = new int[3];

        skillLev[0] = _skillLev1;
        skillLev[1] = _skillLev2;
        skillLev[2] = _skillLev3;
    }

}
