using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public GameManager gameManager;
    public List<Text> Missions;
    void OnEnable()
    {   
        Missions[0].text = (gameManager.RecentMainMission==-1?0:gameManager.RecentMainMission).ToString() + "/10";
        Missions[1].text = (gameManager.RecentSideMission[0] + 1).ToString() + "/4";
        Missions[2].text = (gameManager.RecentSideMission[1] -3).ToString() + "/4";
        Missions[3].text = (gameManager.RecentSideMission[2] - 7).ToString() + "/4";
        Missions[4].text = (gameManager.RecentSideMission[3] - 11).ToString() + "/4";
        Missions[5].text = (gameManager.RecentSideMission[4] - 15).ToString() + "/4";
    }
}
