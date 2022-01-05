using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSound : MonoBehaviour
{
    private AudioSource audioS;
    void Start()
    {
        int rndPosIdx = Random.Range(0, 1);
        audioS = GetComponent<AudioSource>();
        audioS.timeSamples = rndPosIdx;
        audioS.Play();
    }
}
