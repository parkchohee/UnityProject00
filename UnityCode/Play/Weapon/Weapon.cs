using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [HideInInspector]
    public GameObject playerFrom;
    public int AttackPower;

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);

    //    if (other.gameObject.tag == "Player")
    //        return;

    //    var hit = other.gameObject;
    //    var health = hit.GetComponent<Health>();

    //    if (health != null)
    //    {
    //        health.TakeDamage(playerFrom, 10);
    //    }
    //}
}
