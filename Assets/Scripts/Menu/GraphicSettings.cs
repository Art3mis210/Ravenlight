using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphicSettings
{
    public int ResolutionValue;
    public int QualityValue;
    public bool[] Effects;
    public GraphicSettings(Graphics graphics)
    {
        ResolutionValue = graphics.CurrentResolutionValue;
        QualityValue = graphics.CurrentQualityValue;
        Effects = new bool[6];
        Effects = graphics.Effects;
    }
}
