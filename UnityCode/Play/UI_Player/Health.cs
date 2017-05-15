using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Gauge
{
    public GameObject EnemyHealth;

    public bool isEnemy = false;
    public bool isLocalPlayer;

    void Start()
    {
        IsAutoIncreaseGauge = !isEnemy;
        AutoIncreaseTime = 5.0f;

        if (isEnemy)
        {
            EnemyHealth.SetActive(false);
        }

    }

    public void TakeDamage(GameObject playerFrom, int amount)
    {
        if (isEnemy)
        {
            EnemyHealth.SetActive(true);
        }

        CurrentGauge -= amount;
    
        if (CurrentGauge <= 0)
        {
            if (isLocalPlayer)
            {
                GameObject.Find("Controller").GetComponent<PlaySceneUIController>().DiePop.SetActive(true);

                if(this.gameObject.GetComponent<PlayerController>().GetAnimationType() != PlayerController.PlayerAnimationType.NORMAL_DIE)
                {
                    this.gameObject.GetComponent<PlayerController>().SetPlayerAnimationType(PlayerController.PlayerAnimationType.NORMAL_DIE);
                }

                //if (diePop.activeSelf)
                //    this.gameObject.GetComponent<PlayerController>().SetPlayerAnimationType(PlayerController.PlayerAnimationType.NORMAL_DIE);
                //else
                //    diePop.SetActive(true);



                //Respawn(new Vector3(0, 0, 0), new Quaternion(0, 180, 0, 0));
            }
            else
            {
                if (isEnemy)
                {
                    EnemyHealth.SetActive(false);

                    if (this.gameObject.GetComponent<EnemyController>().isDie)
                        return;

                    this.gameObject.GetComponent<EnemyController>().isDie = true;

                    // >> : 죽인 플레이어의 경험치 올려줌..
                    Exp exp = playerFrom.GetComponent<Exp>();
                    exp.IncreaseGauge(gameObject.GetComponent<EnemyController>().expPoint);
                    // << :

                    StartCoroutine(this.RespawnCoroutine());
                }
               // Destroy(this.gameObject);
            }
        }
        
    }

    IEnumerator RespawnCoroutine()
    {
        GameObject EnemyModel = this.gameObject.GetComponentInChildren<Animator>().gameObject;

        yield return new WaitForSeconds(7.0f);

        EnemyController ec = this.gameObject.GetComponent<EnemyController>();
        Respawn(ec.spawnpoint.transform.position, ec.spawnpoint.transform.rotation);
        ec.Respawn();
        EnemyModel.SetActive(true);

    }

    //public void OnChangeHealth()
    //{
    //    if (CurrentGauge <= 0)
    //    {
    //        if (isLocalPlayer)
    //        {
    //            // >> : 나의 체력이 0보다 작음.. 
    //            //    -> 죽은거...
    //            // TODO : 확인누르면 마을에서 리스폰해야함
    //            CurrentGauge = MaxGauge;
    //            Respawn();
    //            // << :
    //        }
    //        else
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }

    //}

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Vector3 spawnPoint = position;
        Quaternion spawnRotation = rotation;
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;

        CurrentGauge = MaxGauge;

        if (isLocalPlayer)
        {
            this.gameObject.GetComponent<PlayerController>().SetPlayerAnimationType(PlayerController.PlayerAnimationType.NORMAL_IDLE);
        }
    }
}
