using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionThree : MainMissions
{
    public int MissionCount = 3;
    private GameManager gameManager;
    public List<Enemy> Enemies;
    public List<RavenlightMember> Prisoners;
    private int MissionPart;
    private int TotalMissionParts;
    public Vector3[] MissionLocation;
    public string[] MissionObjectives;
    public GameObject MissionGiver;
    public int EnemyKilledCount;
    public GameObject EnemyCar;
    public List<AudioClip> Dialogue;
    public AudioClip BackgroundMusic;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.TurnDestinationMarkOnOff(true);
        gameManager.Civilians.SetActive(false);
        MissionPart = -1;
        EnemyCar.SetActive(true);
        TotalMissionParts = MissionObjectives.Length;
        gameManager.missionTrigger.transform.gameObject.SetActive(true);
        gameManager.SetDestination(gameManager.missionTrigger.transform.gameObject);
        ChangeObjectiveAndLocation();
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.gameObject.SetActive(true);
        }
        for (int i = 0; i < Prisoners.Count; i++)
        {
            Prisoners[i].transform.gameObject.SetActive(true);
        }
        gameManager.KilledCount = 0;
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        if (Vector3.Distance(gameManager.missionTrigger.transform.position,gameManager.player.transform.position)<15f)
        {
            if (MissionPart == 0)
            {
                gameManager.BackGroundMusic(null, "Stop");
                gameManager.MainAudioSource.PlayOneShot(Dialogue[MissionPart]);
                for (int i=0;i<Enemies.Count;i++)
                {
                    Enemies[i].enemyBehaviour = Enemy.EnemyBehaviour.FightWithPlayer;
                }
                ChangeObjectiveAndLocation();
            }
        }
        if (MissionPart == 1 && gameManager.KilledCount == Enemies.Count)
        {
            gameManager.MainAudioSource.PlayOneShot(Dialogue[MissionPart]);
            for (int i=0;i<Prisoners.Count;i++)
            {
                Prisoners[i].Target = MissionGiver.transform;
                Prisoners[i].companionBehaviour = RavenlightMember.CompanionBehaviour.GoTowardsTarget;
            }
            Invoke("MissionComplete", 10f);
            MissionPart++;
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
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].transform.gameObject.SetActive(false);
        }
        for (int i = 0; i < Prisoners.Count; i++)
        {
            Prisoners[i].transform.gameObject.SetActive(false);
        }
        gameManager.Companions.SetActive(true);
        gameManager.Civilians.SetActive(true);
        EnemyCar.SetActive(false);
        gameManager.MissionComplete(300);
        this.enabled = false;
    }
}
