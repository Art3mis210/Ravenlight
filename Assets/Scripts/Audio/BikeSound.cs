using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSound : MonoBehaviour
{
    private AudioSource audioS;
    private float orignalPitch;
    bool PitchChanged;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        PitchChanged = false;
    }
    void Update()
    {
        if (Time.timeScale == 0 && PitchChanged==false)
        {
            PitchChanged = true;
            orignalPitch = audioS.pitch;
            audioS.pitch = 0;
        }
        else if(Time.timeScale==1 && PitchChanged==true)
        {
            PitchChanged = false;
            audioS.pitch = orignalPitch;
        }
    }
}
