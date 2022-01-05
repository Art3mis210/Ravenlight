using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSix : MainMissions
{
    public int MissionCount = 6;
    private GameManager gameManager;
    public GameObject Enemies;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject MissionGiver;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;
    public GameObject BikeIndicator;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        Enemies.SetActive(true);
        gameManager.Civilians.SetActive(false);
        MissionPart = -1;
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.PlayerBike.transform.position = MissionLocation[8];
        gameManager.PlayerBike.transform.gameObject.SetActive(false);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
        BikeIndicator.SetActive(false);
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            if (MissionPart <= 7)
            {
                gameManager.MainAudioSource.PlayOneShot(Dialogue[Random.Range(0, 1)]);
                ChangeObjectiveAndLocation();
                if(MissionPart==7)
                {
                    gameManager.PlayerBike.transform.gameObject.SetActive(true);
                    BikeIndicator.SetActive(true);
                }
            }
            else
            {
                gameManager.MainAudioSource.PlayOneShot(Dialogue[2]);
                MissionComplete();
            }
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
        gameManager.Civilians.SetActive(true);
        Enemies.SetActive(false);
        gameManager.BackGroundMusic(BackgroundMusic, "Stop");
        gameManager.MissionComplete(600);
        this.enabled = false;
    }
}

