using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMission : MonoBehaviour
{
    public RandomEvent randomEvent;
    public GameObject Props;
    public GameObject Enemies;
    private bool MissionOver;
    public int EnemiesKilled;
    public GameObject Target;
    void Start()
    {
        MissionOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemiesKilled == Enemies.transform.childCount && MissionOver == false)
        {
            if (Vector3.Distance(transform.position, randomEvent.Player.position) > 200f)
            {
                MissionOver = true;
                MissionCompleted();
            }
            else if(Target.activeInHierarchy)
            {
                Target.SetActive(false);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, randomEvent.Player.position) > 200f)
            {
                Enemies.SetActive(false);
                Props.SetActive(false);
            }
            else
            {
                Enemies.SetActive(true);
                Props.SetActive(true);
            }
        }
    }
    void MissionCompleted()
    {
        MissionOver = true;
        randomEvent.CoolDownTime = 300;
        randomEvent.RandomMissionActive = false;
        if(randomEvent.gameManager.playerInventory.InventoryItems[23]<100)
        {
            randomEvent.gameManager.playerInventory.InventoryItems[23] += 5;
        }
        randomEvent.gameManager.RandomEventOn = false;
        transform.gameObject.SetActive(false);
        
    }
}
