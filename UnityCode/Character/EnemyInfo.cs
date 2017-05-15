using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
    public int EnemyID;
    public string Name;
    public string PrefabName;
    public int Exp;
    public int Hp;
    public int Power;

    public EnemyInfo(int _enemyId, string _name, string _prefabName, int _exp, int _hp, int _power)
    {
        EnemyID = _enemyId;
        Name = _name;
        PrefabName = _prefabName;
        Exp = _exp;
        Hp = _hp;
        Power = _power;
    }
    
}
