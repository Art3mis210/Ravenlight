using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject Buttons;
    public List<GameObject> Menus;
    public void TurnMenuOn(GameObject Menu)
    {
        Buttons.SetActive(false);
        for (int i = 0; i < Menus.Count; i++)
        {
            if (Menus[i] != Menu)
                Menus[i].SetActive(false);
        }
        Menu.SetActive(true);
    }
    public void TurnMenuOff(GameObject Menu)
    {
        Buttons.SetActive(true);
        Menu.SetActive(false);
    }
}
