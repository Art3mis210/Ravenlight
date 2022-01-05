using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerThirdPersonCamera : MonoBehaviour
{
    public List<CinemachineFreeLook> ThirdPersonCamera;
    int CurrentCamera;
    int PreviousCamera;
    private void Start()
    {
        CurrentCamera = 0;
        ThirdPersonCamera[CurrentCamera].Priority = 10;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            PreviousCamera = CurrentCamera;
            ThirdPersonCamera[CurrentCamera].Priority = 0;
            CurrentCamera = (CurrentCamera + 1) % ThirdPersonCamera.Count;
            ThirdPersonCamera[CurrentCamera].Priority = 10;
        }
    }
    public void ChangeTarget(Transform Target)
    {
        for(int i=0;i<ThirdPersonCamera.Count;i++)
        {
            ThirdPersonCamera[i].LookAt = Target;
            ThirdPersonCamera[i].Follow = Target;
        }
    }
}
