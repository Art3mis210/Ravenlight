using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponCamera : MonoBehaviour
{
    public CinemachineVirtualCamera GunCamera;
    public bool Status;
    public void GunCameraOn(bool Status)
    {
        if(Status==true)
            GunCamera.Priority = 21;
        else
            GunCamera.Priority = 0;
        this.Status = Status;
    }
}
