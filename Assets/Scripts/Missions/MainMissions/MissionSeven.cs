using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSeven : MainMissions
{
    public int MissionCount = 7;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public StrongZombie strongZombie;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        MissionPart = -1;
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            if (MissionPart == 0)
            {
                gameManager.MainAudioSource.PlayOneShot(Dialogue[MissionPart]);
                ChangeObjectiveAndLocation();
                gameManager.BackGroundMusic(BackgroundMusic, "Play");
                strongZombie.transform.gameObject.SetActive(true);
            }
        }
        if (MissionPart == 1 && strongZombie.Health == 0)
        {
            gameManager.MainAudioSource.PlayOneShot(Dialogue[MissionPart]);
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
        gameManager.RecentMainMission = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        gameManager.Civilians.SetActive(true);
        gameManager.BackGroundMusic(null, "Stop");
        gameManager.MissionComplete(600);
        this.enabled = false;
    }
}
