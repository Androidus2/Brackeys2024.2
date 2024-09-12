using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapSpawner : MonoBehaviour
{

    [SerializeField]
    private Transform[] scrapPrefabs;

    [SerializeField]
    private int scrapAmount = 75;

    BoxCollider spawnArea;

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider>();

        for (int i = 0; i < scrapAmount; i++)
        {
            int index = Random.Range(0, scrapPrefabs.Length);
            SpawnScrap(index);
        }
    }

    private void SpawnScrap(int index)
    {
        Vector3 spawnPoint = new Vector3(0, -100, 0);

        int maxIterations = 100;

        // Cast a ray downwards and if it hits the ground, spawn the scrap there, otherwise generate another point
        while(maxIterations > 0)
        {
            Ray ray = new Ray(spawnPoint, Vector3.down);
            RaycastHit hit;
            Vector3 potentialValidPoint;

            if(Physics.Raycast(ray, out hit) && CheckAboveSpawnPoint(spawnPoint, out potentialValidPoint))
            {
                spawnPoint = potentialValidPoint;
                break;
            }

            maxIterations--;

            spawnPoint = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                                    Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                                                                                        Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));
        }

        if(maxIterations == 0)
        {
            Debug.LogError("Could not find a valid spawn point for scrap");
        }

        Transform scrap = Instantiate(scrapPrefabs[index], spawnPoint, Quaternion.identity);
        scrap.parent = transform;

        //Generate a random rotation
        scrap.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    bool CheckAboveSpawnPoint(Vector3 spawnPoint, out Vector3 validPositionChange)
    {
        validPositionChange = Vector3.zero;
        // Cast a ray downwards and if it hits the ground, spawn the scrap there, otherwise generate another point

        Ray ray = new Ray(spawnPoint + new Vector3(0, 100, 0), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("We hit something!!!!" + hit.collider.tag + " / " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Ground"))
            {
                validPositionChange = hit.point;
                return true;
            }

            return false;
        }

        return false;
    }

}
