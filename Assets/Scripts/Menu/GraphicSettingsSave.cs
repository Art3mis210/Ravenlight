using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GraphicSettingsSave 
{
    public static void SaveSettings(Graphics graphics)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string Path = Application.persistentDataPath + "/Graphic.settings";
        FileStream stream = new FileStream(Path, FileMode.Create);
        GraphicSettings graphicSettings = new GraphicSettings(graphics);
        formatter.Serialize(stream, graphicSettings);
        stream.Close();
    }
    public static GraphicSettings LoadSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string Path = Application.persistentDataPath + "/Graphic.settings";
        FileStream stream = new FileStream(Path, FileMode.Open);
        GraphicSettings graphicSettings = formatter.Deserialize(stream) as GraphicSettings;
        stream.Close();
        return graphicSettings;
    }
}
