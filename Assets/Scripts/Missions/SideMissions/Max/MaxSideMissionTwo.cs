using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSideMissionTwo : SideMissions
{
    public int MissionCount = 5;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public GameObject Enemies;
    public GameObject Props;
    public AudioClip BackgroundMusic;
    void Start()
    {
        MissionPart = -1;
        gameManager = GetComponent<GameManager>();
        gameManager.KilledCount = 0;
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        Props.SetActive(true);
        Enemies.SetActive(true);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (gameManager.KilledCount == Enemies.transform.childCount)
        {
            MissionComplete();
        }
    }
    void ChangeObjectiveAndLocation()
    {
        MissionPart++;
        gameManager.ChangeObjective(MissionObjectives[MissionPart]);
        gameManager.missionTrigger.ChangeLocation(MissionLocation[MissionPart]);
    }
    void MissionComplete()
    {
        gameManager.RecentSideMission[1] = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        Props.SetActive(false);
        Enemies.SetActive(false);
        gameManager.Civilians.SetActive(true);
        gameManager.MissionComplete(500);
        gameManager.BackGroundMusic(null, "Stop");
        if (gameManager.playerInventory.InventoryItems[3] < 70)
            gameManager.playerInventory.InventoryItems[3] = 70;
        this.enabled = false;
    }
}
