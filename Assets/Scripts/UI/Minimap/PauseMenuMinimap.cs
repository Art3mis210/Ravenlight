using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuMinimap : MonoBehaviour
{
    public GameObject MinimapCamera;
    private void OnEnable()
    {
        MinimapCamera.SetActive(true);
    }
    private void OnDisable()
    {
        MinimapCamera.SetActive(false);
    }
}
