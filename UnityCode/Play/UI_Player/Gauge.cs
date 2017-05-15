using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {

    public float MaxGauge;
    public float CurrentGauge;

    public Image GaugeBar;

    protected float AutoIncreaseTime = 0.0f;
    protected float PassedTime = 0.0f;
    protected bool IsAutoIncreaseGauge = false;
    protected float AutoIncreaseGaugeAmount = 1.0f;

    void Start()
    {

    }

    void Update()
    {

        if (IsAutoIncreaseGauge)
        {
            PassedTime += Time.deltaTime;

            if (PassedTime > AutoIncreaseTime)
            {
                PassedTime = 0;
                IncreaseGauge(AutoIncreaseGaugeAmount);
            }

        }

        if (GaugeBar != null)
            GaugeBar.fillAmount = CurrentGauge / MaxGauge;
    }

    public virtual void IncreaseGauge(float amount)
    {
        CurrentGauge += amount;
        if (CurrentGauge > MaxGauge)
            CurrentGauge = MaxGauge;
    }

    public void SetGauge(int maxGauge, int currentGauge)
    {
        MaxGauge = maxGauge;
        CurrentGauge = currentGauge;
    }

}
