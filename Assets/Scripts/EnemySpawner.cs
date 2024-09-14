using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float timeBetweenEnemies = 60f;

    [SerializeField]
    private int maxEnemyCount = 5;

    [SerializeField]
    private BoxCollider[] enemyWalkAreas;

    private int enemyCount = 2;

    private float nextSpawn = 120f;

    void Update()
    {
        nextSpawn -= Time.deltaTime;
        if (nextSpawn <= 0 && enemyCount < maxEnemyCount)
        {
            enemyCount++;
            nextSpawn = timeBetweenEnemies;
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<Cloud>().SetWalkArea(enemyWalkAreas[Random.Range(0, enemyWalkAreas.Length)]);
            newEnemy.transform.parent = transform.parent;
        }
    }

}
