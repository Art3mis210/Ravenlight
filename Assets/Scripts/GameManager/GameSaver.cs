using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSaver : MonoBehaviour
{
    public GameManager gameManager;
    public Text SaveGameUI;
    private void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
    private void OnTriggerStay(Collider other)
    {
        SaveGameUI.enabled = true;
        if (Input.GetKeyDown(KeyCode.E) )
        {
            if (gameManager.CurrentlyOnMission == false)
            {   
                gameManager.SaveGame();
                SaveGameUI.text = "Game Saved";
            }
            else
            {
                SaveGameUI.text = "Cannot Save During Mission";
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        SaveGameUI.enabled = false;
        SaveGameUI.text = "Press E to Save Game";
    }
}
