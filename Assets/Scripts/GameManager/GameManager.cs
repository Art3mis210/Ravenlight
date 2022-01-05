using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Component[] MainMissionsList;
    public int RecentMainMission=-1;
    public int[] RecentSideMission;
    public Component[] SideMissionsList;
    public Text ObjectiveText;
    public DestinationDirection destinationDirection;
    public Inventory playerInventory;
    public PlayerWeapons playerWeapons;
    public Shop shop;
    public Player player;
    public List<EnemySpawner> EnemySpawners;
    public List<ZombieSpawner> ZombieSpawners;
    public Camera MainCamera;
    public MissionTrigger missionTrigger;
    public Bike PlayerBike;
    public GameObject Civilians;
    public GameObject Companions;
    public GameObject RavenlightWall;
    public bool CurrentlyOnMission;
    public GameObject PauseMenu;
    float PreviousTimeScale;

    public int KilledCount;

    public List<GameObject> MissionGivers;
    public GameObject GameOverScreen;
    private bool GameOver;
    public GameObject MissionCompleteIndication;
    public Text MoneyGained;

    public AudioSource MainAudioSource;
    public AudioSource BackgroundAudioSource;

    public GameObject RandomEvents;
    public GameObject InteractionIndicator;
    public bool EnableCursor;

    public AudioClip GameOverSound;
    public bool RandomEventOn;
    void Start()
    {
        GameOver = false;
        MainMissionsList = gameObject.GetComponents(typeof(MainMissions));
        SideMissionsList = gameObject.GetComponents(typeof(SideMissions));
        LoadGame();
        Time.timeScale = 1;
        RandomEventOn = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && playerInventory.InventoryItems[0]>0)
        {
           if(!PauseMenu.activeInHierarchy)
           {
                PauseMenu.SetActive(true);
                PreviousTimeScale = Time.timeScale;
                Time.timeScale = 0;
           }
           else
           {
                PauseMenu.SetActive(false);
                Time.timeScale = PreviousTimeScale;
           }
        }
        if(GameOver==false && playerInventory.InventoryItems[0]<=0)
        {
            GameOver = true;
            MainAudioSource.loop = false;
            MainAudioSource.clip = GameOverSound; 
            MainAudioSource.Play();
            Invoke("StartGameOver", 5f);
        }
        if((Time.timeScale==1f || Time.timeScale == 0.2f) && EnableCursor==false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

    }
    public void StartGameOver()
    {
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }
    public void ChangeObjective(string Objective)
    {
        ObjectiveText.text = Objective;
    }
    public void SetDestination(GameObject Destination)
    {
        destinationDirection.ChangeTarget(Destination);
    }
    public void TurnDestinationMarkOnOff(bool Status)
    {
        destinationDirection.transform.gameObject.SetActive(Status);
    }
    public void SaveGame()
    {
        LoadSave.SaveGame(playerInventory,playerWeapons,shop,player,this);
    }
    public void LoadGame()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/Ravenlight.save"))
        {
            PlayerData Data = LoadSave.LoadGame();
            playerInventory.LoadGame(Data);
            playerWeapons.LoadGame(Data);
            shop.LoadGame(Data);
            player.LoadGame(Data);
            RecentMainMission = Data.RecentMainMission;
            RecentSideMission = Data.RecentSideMission;
        }
        if (RecentMainMission == -1)
        {
            Component component = GetComponent(MainMissionsList[RecentMainMission + 1].GetType().Name);
            MainMissions mainMission = component as MainMissions;
            mainMission.enabled = true;
            CurrentlyOnMission = true;
        }
        if (RecentMainMission >= 2)
        {
            RandomEvents.SetActive(true);
            PlayerBike.enabled = true;
        }
        if (RecentMainMission >= 3)
        {
            Companions.SetActive(true);
        }
        if (RecentMainMission >= 0)
        {
            for (int i = 0; i < MissionGivers.Count; i++)
            {
                MissionGivers[i].SetActive(true);
            }
        }
        if (RecentMainMission >= 5)
        {
            RavenlightWall.SetActive(true);
        }
    }
    public void SpawnEnemyNearPlayer()
    {
        float Min = 200f;
        for(int i=0;i<EnemySpawners.Count;i++)
        {
            if(Vector3.Distance(player.transform.position,EnemySpawners[i].transform.position)<=Min)
            {
                EnemySpawners[i].SpawnEnemy();
            }
        }
    }
    public void SpawnZombieNearPlayer()
    {
        float Min = 200f;
        for (int i = 0; i < ZombieSpawners.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, ZombieSpawners[i].transform.position) <= Min)
            {
                ZombieSpawners[i].SpawnZombie();
            }
        }
    }
    public void MissionComplete(int Money)
    {
        MoneyGained.text = Money.ToString() + " " + "Ä";
        playerInventory.Ark += Money;
        MissionCompleteIndication.SetActive(true);
    }
    public void BackGroundMusic(AudioClip Clip,string PlayState)
    {
        BackgroundAudioSource.clip = Clip;
        if(PlayState=="Play")
        {
            BackgroundAudioSource.Play();
        }
        else if(PlayState == "Stop")
        {
            BackgroundAudioSource.Stop();
        }
    }
    public void LoadCredits()
    {
        SaveGame();
        Invoke("Credits",10f);
    }
    void Credits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
}
