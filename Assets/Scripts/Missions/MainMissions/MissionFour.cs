using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFour : MainMissions
{
    public int MissionCount = 4;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public GameObject Zombies;
    public AudioClip Dialogue;
    public AudioClip BackgroundMusic;
    void Start()
    {
        MissionPart = -1;
        gameManager=GetComponent<GameManager>();
        gameManager.KilledCount = 0;
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        Zombies.SetActive(true);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }

    void Update()
    {
         if(gameManager.KilledCount == Zombies.transform.childCount)
         {
            gameManager.MainAudioSource.PlayOneShot(Dialogue);
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
        gameManager.MissionComplete(400);
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        Zombies.SetActive(false);
        gameManager.Civilians.SetActive(false);
        gameManager.BackGroundMusic(BackgroundMusic, "Stop");
        this.enabled = false;
    }
}
