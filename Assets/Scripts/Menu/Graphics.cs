using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.IO;

public class Graphics : MonoBehaviour
{
    public int[] ResolutionSetWidth;
    public int[] ResolutionSetHeight;
    public int CurrentResolutionValue;
    public int CurrentQualityValue;
    public bool[] Effects;
    public Toggle[] ToggleEffects;
    public Dropdown ResolutionDropDown;
    public Dropdown QualityDropdown;
    bool fullScreen;
    public PostProcessProfile PPP;
    private void Start()
    {
        Effects = new bool[6];
        if(File.Exists(Application.persistentDataPath + "/Graphic.settings"))
            LoadSettings();
    }
    public void ResolutionChanged(int Value)
    {
        Screen.SetResolution(ResolutionSetWidth[Value], ResolutionSetHeight[Value], fullScreen);
        CurrentResolutionValue = Value;
        SaveSettings();

    }
    public void QualityChanged(int Value)
    {
        QualitySettings.SetQualityLevel(Value);
        CurrentQualityValue = Value;
        SaveSettings();
    }
    public void FullScreenUpdate(bool Toggle)
    {
        Screen.fullScreen = Toggle;
        fullScreen = Toggle;
        Screen.SetResolution(ResolutionSetWidth[CurrentResolutionValue], ResolutionSetHeight[CurrentResolutionValue], fullScreen);
        Effects[0] = Toggle;
        SaveSettings();
    }
    public void BloomChange(bool Toggle)
    {
        PPP.GetSetting<Bloom>().active = Toggle;
        Effects[1] = Toggle;
        SaveSettings();
    }
    public void VignetteChange(bool Toggle)
    {
        PPP.GetSetting<Vignette>().active = Toggle;
        Effects[2] = Toggle;
        SaveSettings();
    }
    public void ChromaticAbberationChange(bool Toggle)
    {
        PPP.GetSetting<ChromaticAberration>().active = Toggle;
        Effects[3] = Toggle;
        SaveSettings();
    }
    public void DepthOfFieldChange(bool Toggle)
    {
        PPP.GetSetting<DepthOfField>().active = Toggle;
        Effects[4] = Toggle;
        SaveSettings();
    }
    public void AmbientOcculsionChange(bool Toggle)
    {
        PPP.GetSetting<AmbientOcclusion>().active = Toggle;
        Effects[5] = Toggle;
        SaveSettings();
    }
    void SaveSettings()
    {
        GraphicSettingsSave.SaveSettings(this);
    }
    void LoadSettings()
    {
        GraphicSettings graphicSettings = GraphicSettingsSave.LoadSettings();
        Effects = graphicSettings.Effects;
        CurrentResolutionValue = graphicSettings.ResolutionValue;
        CurrentQualityValue = graphicSettings.QualityValue;
        for(int i=0;i<6;i++)
        {
            ToggleEffects[i].isOn = Effects[i];
        }
        ResolutionDropDown.value = CurrentResolutionValue;
        QualityDropdown.value = CurrentQualityValue;
    }
}
