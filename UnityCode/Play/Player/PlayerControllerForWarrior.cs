using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForWarrior : PlayerController
{
    public GameObject AxeObject;
    Axe axe;
    Xft.XWeaponTrail axeTrail;

    public override void Start()
    {
        base.Start();

        axe = AxeObject.GetComponent<Axe>();
        axe.playerFrom = this.gameObject;
        axe.AttackPower = AttackPower;
        axe.AxeAttack(false);

        axeTrail = gameObject.GetComponentInChildren<Xft.XWeaponTrail>();
        axeTrail.enabled = false;
    }

    public override void BasicAttack()
    {
        // TODO : network
        animationType = PlayerAnimationType.WORRIOR_BASIC_ATTACK;
        StartCoroutine(AttackCoroutine());
    }

    public override void Skill_1()
    {
        axeTrail.enabled = true;
        axeTrail.MyColor = new Color(255, 0, 0);
        animationType = PlayerAnimationType.WORRIOR_SKILL_ATTACK_1;
        animator.SetInteger("AnimationIndex", animationType.GetHashCode());
        StartCoroutine(AttackCoroutine());
        Debug.Log("1");
    }

    public override void Skill_2()
    {
        axeTrail.enabled = true;
        axeTrail.MyColor = new Color(0, 0, 255);
        animationType = PlayerAnimationType.WORRIOR_SKILL_ATTACK_2;
        animator.SetInteger("AnimationIndex", animationType.GetHashCode());
        StartCoroutine(Skill2Coroutine());
        Debug.Log("2");
    }

    public override void Skill_3()
    {
        axeTrail.enabled = true;
        axeTrail.MyColor = new Color(255, 255, 255);
        animationType = PlayerAnimationType.WORRIOR_SKILL_ATTACK_3;
        animator.SetInteger("AnimationIndex", animationType.GetHashCode());
        StartCoroutine(Skill2Coroutine());
        Debug.Log("3");
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        axe.AxeAttack(true);

        yield return new WaitForSeconds(0.2714285f);

        axeTrail.enabled = false;
        animationType = PlayerAnimationType.NORMAL_IDLE;
    }
    IEnumerator Skill2Coroutine()
    {
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);
        axe.AxeAttack(true);
        yield return new WaitForSeconds(0.1f);

        axeTrail.enabled = false;
        animationType = PlayerAnimationType.NORMAL_IDLE;
    }

}
