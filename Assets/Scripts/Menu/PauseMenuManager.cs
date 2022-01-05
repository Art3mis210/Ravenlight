using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenuManager : MonoBehaviour
{
    public List<GameObject> Menus;
    public GameObject Buttons;
    public GameObject LoadingScreen;
    public Image LoadingBar;
    void Start()
    {
        
    }
    public void TurnMenuOn(GameObject Menu)
    {
        for (int i = 0; i < Menus.Count; i++)
        {
            if (Menus[i] != Menu)
                Menus[i].SetActive(false);
        }
        Menu.SetActive(true);
    }
    public void LoadNewScene(string SceneName)
    {
        Buttons.SetActive(false);
        for (int i = 0; i < Menus.Count; i++)
        {
            Menus[i].SetActive(false);
        }
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(SceneName));
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
