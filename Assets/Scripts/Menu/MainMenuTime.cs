using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTime : MonoBehaviour
{
    private Text TimeText;
    int sysHour;
    int sysMin;
    void Start()
    {
        TimeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        sysHour = System.DateTime.Now.Hour;
        sysMin= System.DateTime.Now.Minute;
        if(sysMin>9)
            TimeText.text = sysHour.ToString() + " : " + sysMin.ToString();
        else
            TimeText.text = sysHour.ToString() + " : 0" + sysMin.ToString();
    }
}
