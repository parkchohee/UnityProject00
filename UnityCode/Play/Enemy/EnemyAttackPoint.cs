using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    bool isAttack;
    int attackPower;

    public void EnemyAttack(bool _isAttack, int _attackPower)
    {
        isAttack = _isAttack;
        attackPower = _attackPower;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAttack)
            return;

        if (other.gameObject.tag != "Player")
            return;

        var hit = other.gameObject;
        var health = hit.GetComponentInChildren<Health>();

        if (health != null)
        {
            health.TakeDamage(null, attackPower);
        }

        isAttack = false;
        Debug.Log( "AttackPoint" + other.gameObject.name + " 공격력" + attackPower);
    }
}
