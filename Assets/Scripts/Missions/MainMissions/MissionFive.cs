using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFive : MainMissions
{

    public int MissionCount = 5;
    private GameManager gameManager;
    public GameObject Enemies;
    public RavenlightMember Companion;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject MissionGiver;
    public GameObject Props;
    public GameObject ConstructionMaterials;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;
    public GameObject Wall;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(true);
        Companion.transform.gameObject.SetActive(true);
        MissionLocation[0] = Companion.transform.position;
        MissionLocation[4] = ConstructionMaterials.transform.position;
        MissionLocation[5] = MissionGiver.transform.position;
        MissionPart = -1;
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (gameManager.missionTrigger.DetectPlayer == true)
        {
            if (MissionPart == 0)
            {
                gameManager.MainAudioSource.PlayOneShot(Dialogue[0]);
                Companion.companionBehaviour = RavenlightMember.CompanionBehaviour.FollowCompanionMode;
                Props.SetActive(true);
                ConstructionMaterials.SetActive(true);
                Enemies.SetActive(true);
                ChangeObjectiveAndLocation();
            }
            else if (MissionPart <=3)
            {
                ChangeObjectiveAndLocation();
            }
            else if(MissionPart==4)
            {
                ConstructionMaterials.SetActive(false);
                gameManager.MainAudioSource.PlayOneShot(Dialogue[1]);
                Companion.GetComponent<RavenlightMember>().companionBehaviour = RavenlightMember.CompanionBehaviour.CompanionMode;
                ChangeObjectiveAndLocation();
            }
            else if(MissionPart==5)
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
        Enemies.SetActive(false);
        Props.SetActive(false);
        Companion.transform.gameObject.SetActive(false);
        Props.SetActive(false);
        gameManager.MissionComplete(300);
        gameManager.BackGroundMusic(null, "Stop");
        Wall.SetActive(true);
        this.enabled = false;
    }
}
