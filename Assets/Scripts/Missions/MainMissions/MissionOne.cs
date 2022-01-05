using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionOne : MainMissions
{
    public int MissionCount = 1;
    private GameManager gameManager;
    public GameObject WeaponStash;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject MissionGiver;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        MissionPart = -1;
        TotalMissionParts = MissionObjectives.Length;
        WeaponStash.SetActive(true);
        MissionLocation[0] = WeaponStash.transform.position;
        MissionLocation[1] = MissionGiver.transform.position;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            gameManager.MainAudioSource.PlayOneShot(Dialogue[MissionPart]);
            if (MissionPart == 0)
            {
                WeaponStash.SetActive(false);
                ChangeObjectiveAndLocation();
            }
            else if(MissionPart==1)
            {
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
        gameManager.MissionComplete(200);
        gameManager.BackGroundMusic(null, "Stop");
        this.enabled = false;
    }
}
