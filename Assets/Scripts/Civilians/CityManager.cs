using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    private bool PlayerInCity;
    public Transform Player;
    private void Update()
    {
        if(Vector3.Distance(transform.position, Player.position)<400f && PlayerInCity==false)
        {
            CivilianActive(true);
        }
        else if(Vector3.Distance(transform.position, Player.position) > 400f && PlayerInCity == true)
        {
            CivilianActive(false);
        }
    }
    void CivilianActive(bool Status)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(Status);
        }
        PlayerInCity = Status;
    }

}
