using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyInfo enemyInfo;

    public GameObject enemy;
    public GameObject spawnPoint;
    public int numberOfEnemies;

    [HideInInspector]
    public List<SpawnPoint> enemySpawnPoints;

    void Start ()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = this.gameObject.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            var spawnRotation = Quaternion.Euler(0f, Random.Range(0, 180), 0f);
            SpawnPoint enemySpawnPoint = (Instantiate(spawnPoint, spawnPosition, spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            enemySpawnPoints.Add(enemySpawnPoint);
        }
    }

    public void SpawnEnemies(/*networking*/)
    {
        int i = 0;
        foreach (SpawnPoint sp in enemySpawnPoints)
        {
            Vector3 position = sp.transform.position;
            Quaternion rotation = sp.transform.rotation;
            GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;

            newEnemy.transform.SetParent(sp.transform);
            newEnemy.name = i + "_Enemy";
            newEnemy.GetComponent<Health>().SetGauge(enemyInfo.Hp, enemyInfo.Hp);
            newEnemy.GetComponent<EnemyController>().expPoint = enemyInfo.Exp;
            newEnemy.GetComponent<EnemyController>().attackPower = enemyInfo.Power;
            newEnemy.GetComponent<EnemyController>().spawnpoint = sp;
            i++;
        }

    }

    public void SetEnemyInfo(EnemyInfo _enemyInfo)
    {
        enemyInfo = _enemyInfo;
    }

}
