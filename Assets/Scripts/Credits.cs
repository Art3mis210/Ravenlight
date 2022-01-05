using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private Camera CreditsCamera;
    public GameObject Level;
    private void Start()
    {
        CreditsCamera = GetComponent<Camera>();
    }
    public void TurnLevelOn()
    {
        Level.SetActive(true);
        CreditsCamera.clearFlags = CameraClearFlags.Skybox;
    }
    public void TurnLevelOff()
    {
        Level.SetActive(false);
        CreditsCamera.clearFlags = CameraClearFlags.SolidColor;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
