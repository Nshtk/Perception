using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour {
    private bool coroutineRunning = false;
    private EnemySpawner enemySpawner;
    private Transform playerRig;

    private void Start() {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerRig = GetComponent<Transform>();
    }

    private IEnumerator RespawnCoroutine() {
        coroutineRunning = true;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemies) {
            enemy.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(5);
        foreach(Enemy enemy in enemies) {
            Destroy(enemy.gameObject);
        }
        yield return new WaitForSeconds(1);
        playerRig.position = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(5);
        enemySpawner.InitiateSpawn();
        yield return new WaitForSeconds(1);
        coroutineRunning = false;
    }

    public void Respawn() {
        if (coroutineRunning) {return;}
        StartCoroutine(RespawnCoroutine());
    }
}
