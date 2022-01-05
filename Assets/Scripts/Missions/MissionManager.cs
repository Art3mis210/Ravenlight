using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameManager gameManager;
    public CinemachineVirtualCamera OverlayCamera;
    public GameObject MissionMenu;
    public bool OverlayStatus;

    public Image MissionImage;
    public List<Sprite> MissionImageList;
    public Text MissionTitle;
    public List<string> MissionTitlesList;
    public Text MissionDescription;
    public List<string> MissionDescriptionList;
    public GameObject InteractionIndicator;
    private void Start()
    {
        OverlayStatus = false;
    }
    private void Update()
    {
        if(gameManager.PlayerBike.OnBike==true && OverlayStatus == true)
        {
            OverlayStatus = false;
            OverlayCamera.Priority = 0;
            MissionMenu.SetActive(false);
            gameManager.EnableCursor = false;
        }
        if (gameManager.RecentMainMission >= 10)
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
                    OverlayCamera.Priority = 25;
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
        if(other.gameObject.GetComponent<Player>())
        {
            gameManager.EnableCursor = false;
            InteractionIndicator.SetActive(false);
            if (OverlayStatus == true)
            {
                OverlayStatus = false;
                OverlayCamera.Priority = 0;
                MissionMenu.SetActive(false);
            }
        }
    }
    public void StartMission()
    {
        Component component=gameManager.GetComponent(gameManager.MainMissionsList[gameManager.RecentMainMission+1].GetType().Name);
        MainMissions mainMission = component as MainMissions;
        mainMission.enabled = true;
        gameManager.CurrentlyOnMission = true;
        OverlayStatus = false;
        OverlayCamera.Priority = 0;
        MissionMenu.SetActive(false);
        gameManager.EnableCursor = false;
    }
    void UpdateDescription()
    {
        MissionImage.sprite = MissionImageList[gameManager.RecentMainMission + 1];
        MissionTitle.text = MissionTitlesList[gameManager.RecentMainMission + 1];
        MissionDescription.text=MissionDescriptionList[gameManager.RecentMainMission + 1];
    }
}
