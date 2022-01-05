using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    private AudioSource audioS;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    void Update()
    {
        audioS.pitch = Time.timeScale;
    }
}
