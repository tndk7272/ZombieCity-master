using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int spawnCount = 10;
    public GameObject monster;

    void Start()
    {
        var spawnPoints = GetComponentsInChildren<SpawnPoint>(true);
        for (int i = 0; i < spawnCount; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPoint = spawnPoints[spawnIndex].transform.position;
            Instantiate(monster, spawnPoint, Quaternion.identity);
        }
    }
}