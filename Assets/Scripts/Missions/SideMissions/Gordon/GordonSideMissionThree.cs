using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonSideMissionThree : MonoBehaviour
{
    public int MissionCount = 6;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public List<GameObject> WeaponStash;
    public AudioClip BackgroundMusic;
    void Start()
    {
        MissionPart = -1;
        TotalMissionParts = MissionLocation.Length;
        gameManager = GetComponent<GameManager>();
        gameManager.KilledCount = 0;
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            if (MissionPart < TotalMissionParts - 1)
            {
                if (MissionPart <= 2)
                    WeaponStash[MissionPart].SetActive(false);
                ChangeObjectiveAndLocation();
            }
            else
                MissionComplete();
        }
    }
    void ChangeObjectiveAndLocation()
    {
        MissionPart++;
        if (MissionPart <= 2)
            WeaponStash[MissionPart].SetActive(true);
        gameManager.ChangeObjective(MissionObjectives[MissionPart]);
        gameManager.missionTrigger.ChangeLocation(MissionLocation[MissionPart]);
    }
    void MissionComplete()
    {
        gameManager.RecentSideMission[2] = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        gameManager.Civilians.SetActive(true);
        gameManager.BackGroundMusic(null, "Stop");
        gameManager.MissionComplete(300);
        if (gameManager.playerInventory.InventoryItems[10] < 8)
        {
            gameManager.playerInventory.InventoryItems[10] = 8;
        }
        if (gameManager.playerInventory.InventoryItems[13] < 8)
        {
            gameManager.playerInventory.InventoryItems[13] = 8;
        }
        if (gameManager.playerInventory.InventoryItems[15] < 8)
        {
            gameManager.playerInventory.InventoryItems[15] = 8;
        }
        if (gameManager.playerInventory.InventoryItems[17] < 8)
        {
            gameManager.playerInventory.InventoryItems[17] = 8;
        }
        this.enabled = false;
    }
}
