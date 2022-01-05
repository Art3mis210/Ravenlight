using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTwo : MainMissions
{
    public int MissionCount = 2;
    private GameManager gameManager;
    public GameObject ToolKit;
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
        ToolKit.SetActive(true);
        MissionLocation[0] = ToolKit.transform.position;
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
                ToolKit.SetActive(false);
                ChangeObjectiveAndLocation();
            }
            else if (MissionPart == 1)
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
        gameManager.BackGroundMusic(null, "Stop");
        gameManager.RecentMainMission = MissionCount;
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        gameManager.MissionComplete(200);
        this.enabled = false;
    }
}
