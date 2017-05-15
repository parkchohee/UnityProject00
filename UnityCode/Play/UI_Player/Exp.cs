using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : Gauge
{
    void Start()
    {

    }

    public override void IncreaseGauge(float amount)
    {
        CurrentGauge += amount;
        PlaySceneController controller = GameObject.Find("Controller").GetComponent<PlaySceneController>();
        
        // >> : 레벨업...
        if (CurrentGauge >= MaxGauge)
        {
            CurrentGauge = CurrentGauge - MaxGauge;
            controller.LevelUp((int)CurrentGauge);

        }
        else
        {
            controller.ExpUp((int)CurrentGauge);

        }
        // << :
    }
}
