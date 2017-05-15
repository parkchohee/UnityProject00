using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int Price;

	void Start ()
    {
        Debug.Log("코인생성!!!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlaySceneController pc = GameObject.Find("Controller").GetComponent<PlaySceneController>();
            pc.GetMoney(Price);
            Destroy(this.gameObject);
        }
    }

}
