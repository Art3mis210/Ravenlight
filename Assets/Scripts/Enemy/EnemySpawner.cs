using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> Enemies;
    public GameObject Player;
    public int EnemyCount;
    public List<GameObject> EnemySpawned;
    public int SpawnedEnemyCount;
    void Awake()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject enemy = (GameObject)Instantiate(Enemies[Random.Range(0, Enemies.Count)].transform.gameObject, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
            enemy.transform.parent = transform;
            Enemy EnemyScript = enemy.GetComponent<Enemy>();
            EnemyScript.enemyBehaviour = Enemy.EnemyBehaviour.FindAndHuntPlayer;
            EnemyScript.Player = Player;
            EnemyScript.Target = Player.transform;
            EnemyScript.Spawner = this;
            EnemySpawned.Add(enemy);
            enemy.SetActive(false);
        }
    }
    public void RespawnEnemy(Enemy enemy)
    {
        enemy.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        enemy.GetComponent<Collider>().enabled = true;
        enemy.GetComponent<NavMeshAgent>().enabled = true;
        enemy.GetComponent<Animator>().enabled = true;
        enemy.Ragdoll(false);
        enemy.GetComponent<Collider>().enabled = true;
        enemy.transform.gameObject.SetActive(true);
        enemy.StateVis.enabled = true;

    }
    public void SpawnEnemy()
    {
        if(!EnemySpawned[SpawnedEnemyCount].activeInHierarchy)
        {
            EnemySpawned[SpawnedEnemyCount].SetActive(true);
        }
        SpawnedEnemyCount = (SpawnedEnemyCount + 1) % EnemySpawned.Count;
    }

}
