using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelSideMissionOne : SideMissions
{
    public int MissionCount = 0;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public GameObject Enemies;
    public GameObject Ellie;
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
        Enemies.SetActive(true);
        Ellie.SetActive(true);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }

    void Update()
    {
        if (gameManager.KilledCount == Enemies.transform.childCount && gameManager.missionTrigger.DetectPlayer == true)
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
        gameManager.RecentSideMission[0] = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        Enemies.SetActive(false);
        gameManager.Civilians.SetActive(true);
        Ellie.SetActive(false);
        gameManager.BackGroundMusic(BackgroundMusic, "Stop");
        gameManager.MissionComplete(600);
        if (gameManager.playerInventory.InventoryItems[1] < 60)
            gameManager.playerInventory.InventoryItems[1] = 60;
        this.enabled = false;
    }
}
