using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : Gauge
{
    static float timeHP = 0.0f;

    void Start()
    {
        IsAutoIncreaseGauge = true;
        AutoIncreaseTime = 3.0f;
    }

}
