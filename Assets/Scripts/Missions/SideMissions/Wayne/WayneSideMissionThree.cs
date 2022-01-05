using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayneSideMissionThree : SideMissions
{
    public int MissionCount = 6;
    public string[] MissionObjectives;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public GameObject Drones;
    public float TimeTaken;
    public AudioClip BackgroundMusic;
    void Start()
    {
        TimeTaken = 0f;
        Drones.SetActive(true);
        TotalMissionParts = Drones.transform.childCount - 1;
        gameManager = GetComponent<GameManager>();
        gameManager.KilledCount = 0;
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        gameManager.ChangeObjective(MissionObjectives[0]);
        MissionPart = -1;
        ChangeObjectiveAndLocation();
        gameManager.BackGroundMusic(BackgroundMusic, "Play");

    }
    void Update()
    {

        if (!Drones.transform.GetChild(MissionPart).transform.gameObject.activeInHierarchy)
        {
            ChangeObjectiveAndLocation();
        }
        else
            TimeTaken += Time.deltaTime;
    }
    void ChangeObjectiveAndLocation()
    {
        MissionPart++;
        if (MissionPart <= TotalMissionParts)
        {
            Drones.transform.GetChild(MissionPart).transform.gameObject.SetActive(true);
            gameManager.ChangeObjective(MissionObjectives[0]);
            gameManager.missionTrigger.ChangeLocation(Drones.transform.GetChild(MissionPart).transform.position);
        }
        else
            MissionComplete();

    }
    void MissionComplete()
    {
        gameManager.RecentSideMission[2] = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        gameManager.Civilians.SetActive(true);
        gameManager.BackGroundMusic(null, "Stop");
        if (TimeTaken < 600)
            gameManager.MissionComplete(700);
        else
            gameManager.MissionComplete(500);
        if (gameManager.playerInventory.InventoryItems[5] < 800)
            gameManager.playerInventory.InventoryItems[5] = 800;
        if (gameManager.playerInventory.InventoryItems[7] < 800)
            gameManager.playerInventory.InventoryItems[7] = 800;
        this.enabled = false;
    }
}
