using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class LoadSave 
{
    public static void SaveGame(Inventory playerInventory,PlayerWeapons Weapons,Shop shop,Player player,GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string Path = Application.persistentDataPath + "/Ravenlight.save";
        FileStream stream = new FileStream(Path, FileMode.Create);
        PlayerData data = new PlayerData(playerInventory,Weapons,shop,player, gameManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadGame()
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        string Path = Application.persistentDataPath + "/Ravenlight.save";
        FileStream stream = new FileStream(Path, FileMode.Open);
        PlayerData data=formatter.Deserialize(stream) as PlayerData;
        stream.Close();
        return data;
    }
}
