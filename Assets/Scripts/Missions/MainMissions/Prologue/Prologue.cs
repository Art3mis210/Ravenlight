using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prologue : MainMissions
{
    public int MissionCount = 0;
    private GameManager gameManager;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject Adam;
    public List<AudioClip> Dialogues;
    public GameObject Cutscene;
    public List<AudioClip> PlayerDialogues;
    public AudioClip BackgroundMusic;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        MissionPart = -1;
        gameManager.TurnDestinationMarkOnOff(true);
        TotalMissionParts = MissionObjectives.Length;
        MissionLocation[2] = gameManager.PlayerBike.transform.position;
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        gameManager.Civilians.SetActive(false);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");

    }
    void Update()
    {
        if(gameManager.missionTrigger.DetectPlayer==true)
        {
            if(MissionPart<4)
            {
                ChangeObjectiveAndLocation();
            }
        }
        if(Adam.activeInHierarchy)
        {
            if(Vector3.Distance(Adam.transform.position,gameManager.player.transform.position)<3f && MissionPart==4)
            {
                Adam.GetComponent<RavenlightMember>().companionBehaviour = RavenlightMember.CompanionBehaviour.StandIdle;
                Adam.GetComponent<AudioSource>().PlayOneShot(Dialogues[0]);
                ChangeObjectiveAndLocation();
                StartCoroutine(PlayNextDialogue(Adam.GetComponent<AudioSource>(), Dialogues[1],3f));
            }
            if (Vector3.Distance(Adam.transform.position, gameManager.missionTrigger.transform.position) < 3f && MissionPart == 5)
            {
                Adam.GetComponent<RavenlightMember>().companionBehaviour = RavenlightMember.CompanionBehaviour.StandIdle;
                if(Vector3.Distance(Adam.transform.position, gameManager.player.transform.position)<2f)
                {
                    gameManager.BackGroundMusic(null, "Stop");
                    Cutscene.SetActive(true);
                    gameManager.RecentMainMission = 0;
                    this.enabled = false;
                }
            }
        }
    }
    void ChangeObjectiveAndLocation()
    {
        MissionPart++;
        if (MissionPart<5)
            gameManager.MainAudioSource.PlayOneShot(PlayerDialogues[MissionPart]);
        if (MissionPart == 4)
        {
            for (int i = 0; i < 30; i++)
                gameManager.SpawnZombieNearPlayer();
            gameManager.ChangeObjective(null);
            Adam.SetActive(true);
        }
        gameManager.ChangeObjective(MissionObjectives[MissionPart]);
        gameManager.missionTrigger.ChangeLocation(MissionLocation[MissionPart]);
    }
    IEnumerator PlayNextDialogue(AudioSource AudS,AudioClip AudC,float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        AudS.PlayOneShot(AudC);
        if (MissionPart == 5)
        {
            Adam.GetComponent<RavenlightMember>().Target = gameManager.missionTrigger.transform;
            Adam.GetComponent<RavenlightMember>().companionBehaviour = RavenlightMember.CompanionBehaviour.GoTowardsTarget;
        }

    }
}
