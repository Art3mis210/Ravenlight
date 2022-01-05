using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAccessories : MonoBehaviour
{
    public GameObject Buy;
    public GameObject Max;
    public Shop shop;
    public int AccessoriesAddress0;
    public int AccessoriesAddress1;
    private void OnEnable()
    {
        if(shop.playerInventory.InventoryItems[AccessoriesAddress0] < shop.playerInventory.InventoryItems[AccessoriesAddress1])
        {
            Buy.SetActive(true);
            Max.SetActive(false);
        }
        else
        {
            Buy.SetActive(false);
            Max.SetActive(true);
        }
    }
    public void BuyAccessory(int Ark)
    {
            if (shop.playerInventory.InventoryItems[AccessoriesAddress0] < shop.playerInventory.InventoryItems[AccessoriesAddress1])
            {
                if (shop.playerInventory.Ark >= Ark)
                {
                    shop.playerInventory.Ark -= Ark;
                    shop.playerInventory.InventoryItems[AccessoriesAddress0]++;
                    Debug.Log(shop.playerInventory.InventoryItems[AccessoriesAddress0]);
                }
            }
            if (shop.playerInventory.InventoryItems[AccessoriesAddress0] == shop.playerInventory.InventoryItems[AccessoriesAddress1])
            {
                Buy.SetActive(false);
                Max.SetActive(true);
            }
    }
}
