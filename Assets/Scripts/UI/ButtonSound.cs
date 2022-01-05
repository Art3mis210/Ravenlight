using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip ButtonSelectSound;
    public AudioClip ButtonPressSound;
    private AudioSource audioS;
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    public void ButtonSelect()
    {
        audioS.PlayOneShot(ButtonSelectSound);
    }
    public void ButtonPress()
    {
        audioS.PlayOneShot(ButtonPressSound);
    }
}
