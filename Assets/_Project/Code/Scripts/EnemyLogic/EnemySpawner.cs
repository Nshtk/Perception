using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesAmount = 3;

    private List<int> usedSpawnIndexes;

    private void Start() {
        InitiateSpawn();
    }

    public void InitiateSpawn() {
        usedSpawnIndexes = new List<int>();
        if (enemiesAmount > spawnPoints.Length) {
            Debug.Log("Number of monster spawns is greater than amount of spawners on map");
        }
        else{
            for (int i = 0; i < enemiesAmount; ++i) {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy(){
        int randomIndex = Random.Range(0, spawnPoints.Length);
        while (usedSpawnIndexes.Contains(randomIndex)){
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        usedSpawnIndexes.Add(randomIndex);
        Transform spawnPoint = spawnPoints[randomIndex];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
