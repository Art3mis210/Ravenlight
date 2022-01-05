using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int[] InventoryItems;
    public int[] Weapons;
    public bool[] Owned;
    public int Ark;
    public float[] Position;
    public int RecentMainMission;
    public int[] RecentSideMission;
    public PlayerData(Inventory inventory,PlayerWeapons playerWeapons,Shop shop,Player player,GameManager gameManager)
    {
        InventoryItems = new int[24];
        for(int i=0;i<24;i++)
            InventoryItems[i] = inventory.InventoryItems[i];
        Ark = inventory.Ark;
        Weapons = new int[3];
        for(int i=0;i<playerWeapons.PrimaryWeapons.Count;i++)
        {
            if(playerWeapons.Weapons[1]== playerWeapons.PrimaryWeapons[i])
            {
                Weapons[1] = i;
                break;
            }
        }
        for (int i = 0; i < playerWeapons.SecondaryWeapons.Count; i++)
        {
            if (playerWeapons.Weapons[2] == playerWeapons.SecondaryWeapons[i])
            {
                Weapons[2] = i;
                break;
            }
        }
        Owned = shop.Owned;
        Position=new float[3];
        Position[0] = player.transform.position.x;
        Position[1] = player.transform.position.y;
        Position[2] = player.transform.position.z;
        RecentMainMission = gameManager.RecentMainMission;
        RecentSideMission = gameManager.RecentSideMission;
}
}
