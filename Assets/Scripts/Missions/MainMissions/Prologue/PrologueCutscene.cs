using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PrologueCutscene : MonoBehaviour
{
    private AudioSource AudioS;
    public AudioClip BackgroundMusic;
    public GameManager gameManager;
    public GameObject Hud;
    public GameObject BlackFade;
    public GameObject Logo;
    public GameObject Adam;

    private void Start()
    {
        AudioS = GetComponent<AudioSource>();
        AudioS.PlayOneShot(BackgroundMusic);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayDialogue(AudioClip AudioC)
    {
        AudioS.PlayOneShot(AudioC);
    }
    public void TurnUIOff(int i)
    {
        if(i==0)
            Hud.SetActive(false);
        else
            Hud.SetActive(true);
    }
    public void FadeScreen()
    {
        BlackFade.SetActive(true);
    }
    public void TurnOnLogo()
    {
        Logo.SetActive(true);
    }
    public void TurnGameObjectOff()
    {
        transform.gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        Invoke("SwitchToPlayer", 5f);
        
    }
    void SwitchToPlayer()
    {
        gameManager.RecentMainMission = 0;
        Adam.SetActive(false);
        gameManager.ObjectiveText.text = null;
        gameManager.CurrentlyOnMission = false;
        gameManager.TurnDestinationMarkOnOff(false);
        for(int i=0;i<gameManager.MissionGivers.Count;i++)
        {
            gameManager.MissionGivers[i].SetActive(true);
        }
        Logo.SetActive(false);
        BlackFade.SetActive(false);
        transform.gameObject.SetActive(false);
        Hud.SetActive(true);
        gameManager.MissionComplete(10);
        this.enabled = false;
    }
}
