using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    bool isAttack;

    public void AxeAttack(bool _isAttack)
    {
        isAttack = _isAttack;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAttack)
            return;

        if (other.gameObject.tag != "Enemy")
            return;

        Debug.Log(other.gameObject.name);

        var hit = other.gameObject;
        var health = hit.GetComponentInChildren<Health>();

        if (health != null)
        {
            health.TakeDamage(playerFrom, AttackPower);
        }

        isAttack = false;
    }
}
