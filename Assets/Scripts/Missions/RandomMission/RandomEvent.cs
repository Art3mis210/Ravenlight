using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    public Transform Player;
    public List<GameObject> RandomMissions;
    public bool RandomMissionActive;
    private int RandomMission;
    private List<int> RandomMissionCompleted;
    public GameManager gameManager;
    public float CoolDownTime;
    void Start()
    {
        RandomMissionActive = false;
        RandomMissionCompleted = new List<int>();
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) < 200f && gameManager.RandomEventOn==false)
        {
            if(RandomMissionActive==false && RandomMissionCompleted.Count!=RandomMissions.Count && !gameManager.CurrentlyOnMission && (int)CoolDownTime == 0)
            {
                RandomMission = Random.Range(0, RandomMissions.Count - 1);
                int Count = 0;
                while(RandomMissionCompleted.Contains(RandomMission)==true)
                {
                    if (Count == 5)
                        return;
                    RandomMission = Random.Range(0, RandomMissions.Count - 1);
                    Count++;
                }
                RandomMissionActive = true;
                RandomMissions[RandomMission].SetActive(true);
                gameManager.RandomEventOn = true;
            }
        }
        if((int)CoolDownTime!=0)
        {
            CoolDownTime -= Time.deltaTime;
        }
    }
}
