using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEight : MainMissions
{
    public int MissionCount = 8;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject MissionGiver;
    public GameObject DroneBox;
    public GameObject Enemy;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;

    void Start()
    {
        MissionLocation[1] = DroneBox.transform.position;
        MissionLocation[2] = MissionGiver.transform.position;
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        MissionPart = -1;
        Enemy.SetActive(true);
        DroneBox.SetActive(true);
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.KilledCount = 0;
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            if (MissionPart == 1)
            {
                DroneBox.SetActive(false);
                gameManager.MainAudioSource.PlayOneShot(Dialogue[1]);
                ChangeObjectiveAndLocation();
            }
            else if (MissionPart == 2)
            {
                MissionComplete();
            }
        }
        if (MissionPart == 0 && gameManager.KilledCount == Enemy.transform.childCount)
        {
            ChangeObjectiveAndLocation();
            gameManager.MainAudioSource.PlayOneShot(Dialogue[0]);
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
        gameManager.RecentMainMission = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        Enemy.SetActive(false);
        gameManager.Civilians.SetActive(true);
        gameManager.MissionComplete(300);
        gameManager.BackGroundMusic(null, "Stop");
        this.enabled = false;
    }
}
