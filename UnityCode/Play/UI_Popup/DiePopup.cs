using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePopup : MonoBehaviour
{
    void OK()
    {
        GameObject.FindWithTag("Player").GetComponent<Health>().Respawn(new Vector3(44.8f,2f,-11.7f),new Quaternion(0,180,0,0));
        this.gameObject.SetActive(false);
    }
}
