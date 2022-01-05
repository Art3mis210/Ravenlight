using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianSpawner : MonoBehaviour
{
    public List<Civilian> Civilians;
    public Targets target;
    public int CivilianCount;
    public GameManager gameManager;
    void Awake()
    {
        for (int i = 0; i < CivilianCount; i++)
        {
            GameObject Civ = (GameObject)Instantiate(Civilians[Random.Range(0, Civilians.Count-1)].transform.gameObject,transform.position+new Vector3(Random.Range(-2f,2f),0, Random.Range(-2f, 2f)),Quaternion.identity);
            Civ.transform.parent = transform;
            Civ.GetComponent<Civilian>().PlayerAnimator = gameManager.player.GetComponent<Animator>();
            int Behaviour = Random.Range(-1, 1);
            if (Behaviour == -1)
                Civ.GetComponent<Civilian>().civilianBehaviour = Civilian.CivilianBehaviour.Sad;
            else if(Behaviour==0)
                Civ.GetComponent<Civilian>().civilianBehaviour = Civilian.CivilianBehaviour.Neutral;
            else if(Behaviour==1)
                Civ.GetComponent<Civilian>().civilianBehaviour = Civilian.CivilianBehaviour.Happy;
            Civ.GetComponent<Civilian>().Target = target.AssignTarget();
            Civ.GetComponent<Civilian>().Spawner = this;
        }
    }
    public void SpawnCivilian(Civilian Civ)
    {
        Civ.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        Civ.transform.gameObject.SetActive(true);
        Civ.GetComponent<Collider>().enabled = true;
        Civ.GetComponent<NavMeshAgent>().enabled = true;
        Civ.GetComponent<Animator>().enabled = true;
        Civ.Ragdoll(false);
        gameManager.SpawnEnemyNearPlayer();
    }
}
