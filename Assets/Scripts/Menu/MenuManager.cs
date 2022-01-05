using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public CinemachineVirtualCamera CurrentCamera;
    public List<GameObject> Menus;
    public GameObject Buttons;
    public GameObject LoadingScreen;
    public Image LoadingBar;
    public GameObject ContinueButton;
    public AudioMixer audioMixer;
    private void Start()
    {
        float Mvolume = PlayerPrefs.GetFloat("Volume", 1f);
        audioMixer.SetFloat("Volume", Mvolume);
        Time.timeScale = 1;
        string Path = Application.persistentDataPath + "/Ravenlight.save";
        if(ContinueButton!=null)
            ContinueButton.SetActive(File.Exists(Path));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MenuButtons(CinemachineVirtualCamera NextCamera)
    {
        CurrentCamera.Priority = 0;
        NextCamera.Priority = 1;
        CurrentCamera = NextCamera;
    }
    public void TurnMenuOn(GameObject Menu)
    {
        for(int i=0;i<Menus.Count;i++)
        {
            if(Menus[i]!=Menu)
                Menus[i].SetActive(false);
        }
        Menu.SetActive(true);
    }
    public void TurnMenuOff(GameObject Menu)
    {
        Menu.SetActive(false);
    }
    public void ContinueGame()
    {
        Buttons.SetActive(false);
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadYourAsyncScene("Xark"));
    }
    public void NewGame()
    {
        string Path = Application.persistentDataPath + "/Ravenlight.save";
        if (File.Exists(Path))
        {
            File.Delete(Path);
        }
        Buttons.SetActive(false);
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadYourAsyncScene("Xark"));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadCredits()
    {
        StartCoroutine(LoadYourAsyncScene("Credits"));
    }
    IEnumerator LoadYourAsyncScene(string SceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (asyncLoad.progress < 1)
        {
            LoadingBar.fillAmount = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
        }
    }

}
