using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SideMissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public CinemachineVirtualCamera OverlayCamera;
    public GameObject MissionMenu;
    public bool OverlayStatus;
    public Text Title;
    public Text Description;
    public Image Frame;
    public List<Sprite> MissionImage;
    public List<string> MissionTitles;
    public List<string> MissionDescription;
    public int SideMissionType;
    public int Offset;
    public GameObject InteractionIndicator;
    private void Start()
    {
        OverlayStatus = false;
    }
    private void Update()
    {
        if (gameManager.PlayerBike.OnBike == true && OverlayStatus == true)
        {
            OverlayStatus = false;
            OverlayCamera.Priority = 0;
            MissionMenu.SetActive(false);
            gameManager.EnableCursor = false;
        }
        if (gameManager.RecentSideMission[SideMissionType] >= Offset + 3)
            transform.parent.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() && !gameManager.CurrentlyOnMission)
        {
            UpdateDescription();
            if (OverlayStatus == false)
                InteractionIndicator.SetActive(true); 
            else
                InteractionIndicator.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (OverlayStatus == false)
                {
                    gameManager.EnableCursor = true;
                    OverlayStatus = true;
                    OverlayCamera.Priority = 15;
                    MissionMenu.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (OverlayStatus == true)
                {
                    gameManager.EnableCursor = false;
                    OverlayStatus = false;
                    OverlayCamera.Priority = 0;
                    MissionMenu.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            if (OverlayStatus == true)
            {
                gameManager.EnableCursor = false;
                InteractionIndicator.SetActive(false);
                OverlayStatus = false;
                OverlayCamera.Priority = 0;
                MissionMenu.SetActive(false);
            }
        }
    }
    public void StartMission()
    {
        Component component = gameManager.GetComponent(gameManager.SideMissionsList[gameManager.RecentSideMission[SideMissionType] + 1].GetType().Name);
        SideMissions sideMission = component as SideMissions;
        sideMission.enabled = true;
        gameManager.CurrentlyOnMission = true;
        OverlayStatus = false;
        OverlayCamera.Priority = 0;
        MissionMenu.SetActive(false);
        gameManager.EnableCursor = false;
    }
    void UpdateDescription()
    {
        Frame.sprite = MissionImage[gameManager.RecentSideMission[SideMissionType]-Offset + 1];
        Title.text = MissionTitles[gameManager.RecentSideMission[SideMissionType]-Offset + 1];
        Description.text = MissionDescription[gameManager.RecentSideMission[SideMissionType]-Offset + 1];
    }
}

