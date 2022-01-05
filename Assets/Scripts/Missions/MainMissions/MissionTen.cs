using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTen : MainMissions
{
    public int MissionCount = 10;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject Enemy;
    public GameObject MainBoss;
    public GameObject Companions;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        MissionPart = -1;
        Enemy.SetActive(true);
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(MainBoss.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.KilledCount = 0;
        Companions.SetActive(true);
        gameManager.MainAudioSource.PlayOneShot(Dialogue[0]);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");

    }
    void Update()
    {
        if (MissionPart == 0 && gameManager.KilledCount == Enemy.transform.childCount)
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
        gameManager.RecentMainMission = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        gameManager.Civilians.SetActive(true);
        gameManager.MissionGivers[0].SetActive(false);
        gameManager.MainAudioSource.PlayOneShot(Dialogue[1]);
        gameManager.BackGroundMusic(null, "Stop");
        gameManager.MissionComplete(1000);
        gameManager.LoadCredits();
        this.enabled = false;
        
    }
}
