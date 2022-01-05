using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNine : MainMissions
{
    public int MissionCount = 3;
    public GameObject PlayerDrone;
    private GameManager gameManager;
    public GameObject DroneHud;
    public GameObject PlayerUI;
    public GameObject EnemyDrones;
    public int DestroyedDrones;
    public AudioClip Dialogue;
    public AudioClip BackgroundMusic;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        PlayerDrone.SetActive(true);
        DroneHud.SetActive(true);
        PlayerUI.SetActive(false);
        EnemyDrones.SetActive(true);
        gameManager.player.GetComponent<Player>().enabled = false;
        DestroyedDrones = 0;
        gameManager.MainAudioSource.PlayOneShot(Dialogue);
        gameManager.BackGroundMusic(BackgroundMusic, "Play");
    }
    void Update()
    {
        Debug.Log(EnemyDrones.GetComponentsInChildren<Transform>().GetLength(0));
        if(EnemyDrones.GetComponentsInChildren<Transform>().GetLength(0)<=1)
        {
            MissionComplete();
        }
    }
    void MissionComplete()
    {
        gameManager.RecentMainMission = MissionCount;
        gameManager.CurrentlyOnMission = false;
        PlayerDrone.SetActive(false);
        DroneHud.SetActive(false);
        PlayerUI.SetActive(true);
        EnemyDrones.SetActive(false);
        gameManager.player.GetComponent<Player>().enabled = true;
        gameManager.MissionComplete(250);
        gameManager.BackGroundMusic(null, "Stop");
        this.enabled = false;
    }
}
