using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public List<Zombie> Zombies;
    public GameObject Player;
    public int ZombieCount;
    public List<GameObject> ZombieSpawned;
    public int SpawnedZombieCount;
    public List<GameObject> ZombiesToBeRespawned;
    private bool PlayerNear;
    void Awake()
    {
        for (int i = 0; i < ZombieCount; i++)
        {
            GameObject Zombie = (GameObject)Instantiate(Zombies[Random.Range(0, Zombies.Count)].transform.gameObject, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
            Zombie.transform.parent = transform;
            Zombie ZombieScript = Zombie.GetComponent<Zombie>();
            ZombieScript.Target = Player;
            ZombieScript.Spawner = this;
            ZombieSpawned.Add(Zombie);
            Zombie.SetActive(false);
        }
    }
    public void StartRespawn(Zombie zombie)
    {
        zombie.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        zombie.transform.GetComponent<Collider>().enabled = true;
        zombie.transform.GetComponent<NavMeshAgent>().enabled = true;
        zombie.transform.GetComponent<Animator>().enabled = true;
        zombie.transform.gameObject.SetActive(false);
        ZombiesToBeRespawned.Add(zombie.transform.gameObject);
    }
    public void RespawnZombie()
    {
        for(int i=0;i<ZombiesToBeRespawned.Count;i++)
        {
            if(ZombiesToBeRespawned[i]!=null)
            {
                ZombiesToBeRespawned[i].SetActive(true);
                ZombiesToBeRespawned.RemoveAt(i);
            }
        }
    }
    public void SpawnZombie()
    {
        if (!ZombieSpawned[SpawnedZombieCount].activeInHierarchy)
        {
            ZombieSpawned[SpawnedZombieCount].SetActive(true);
        }
        SpawnedZombieCount = (SpawnedZombieCount + 1) % ZombieSpawned.Count;
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, Player.transform.position) < 200f && PlayerNear==false)
        {
            PlayerNear = true;
            //EnableDisableZombies(true);
        }
        else if(Vector3.Distance(transform.position, Player.transform.position) > 200f && PlayerNear == true)
        {
            PlayerNear = false;
            RespawnZombie();
            EnableDisableZombies(false);

        }
    }
    void EnableDisableZombies(bool Status)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(Status);
        }
    }

}

