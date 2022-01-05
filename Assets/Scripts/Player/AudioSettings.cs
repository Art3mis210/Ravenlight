using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider Volume;
    private void Start()
    {
        float Mvolume = PlayerPrefs.GetFloat("Volume", 1f);
        Volume.value = Mvolume;
        audioMixer.SetFloat("Volume", Volume.value);
    }
    //    public AudioMixerGroup AudioMixer_gp;
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
