using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaneSideMissionOne : SideMissions
{
        public int MissionCount = 8;
        public Vector3[] MissionLocation;
        public string[] MissionObjectives;
        private GameManager gameManager;
        private int MissionPart;
        private int TotalMissionParts;
        public GameObject Enemies;
        public GameObject Supplies;
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
            Supplies.SetActive(true);
            Enemies.SetActive(true);
            gameManager.BackGroundMusic(BackgroundMusic, "Play");
        }
        void Update()
        {
            if (0 == Supplies.transform.childCount)
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
            gameManager.RecentSideMission[2] = MissionCount;
            gameManager.ObjectiveText.text = null;
            gameManager.CurrentlyOnMission = false;
            gameManager.TurnDestinationMarkOnOff(false);
            Supplies.SetActive(false);
            Enemies.SetActive(false);
            gameManager.Civilians.SetActive(true);
            gameManager.BackGroundMusic(null, "Stop");
            gameManager.MissionComplete(400);
            if (gameManager.playerInventory.InventoryItems[9] < 6)
                gameManager.playerInventory.InventoryItems[9] = 6;
            if (gameManager.playerInventory.InventoryItems[21] < 6)
                gameManager.playerInventory.InventoryItems[21] = 6;
        this.enabled = false;
        }
}
