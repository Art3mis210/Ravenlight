using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Shop : MonoBehaviour
{
    public List<GameObject> WeaponsOnShop;
    public List<GameObject> WeaponsOnPlayer;
    public bool[] Owned;
    public List<int> Price;
    public PlayerWeapons playerWeapons;
    public Inventory playerInventory;
    public CinemachineVirtualCamera Cam;
    public bool InShop;
    public GameObject ShopUI;
    public Text Ark;

    private void Start()
    {
        Owned = new bool[5];
    }
    void Update()
    {
        if(InShop==true && Cam.enabled==false)
        {
            Cam.enabled = true;
            ShopUI.SetActive(true);
        }
        else if(InShop == false && Cam.enabled == true)
        {
            Cam.enabled = false;
            ShopUI.SetActive(false);
        }
        Ark.text = playerInventory.Ark.ToString();
    }
    public void EquipPrimaryWeapon(GameObject Weapon)
    {
        if (playerWeapons.Weapons[1].activeInHierarchy)
        {
            playerWeapons.Weapons[1] = Weapon;
            playerWeapons.CurrentWeapon.SetActive(false);
            playerWeapons.CurrentWeapon = Weapon;
            Weapon.SetActive(true);
        }
        else
        {
            playerWeapons.Weapons[1] = Weapon;
        }
        
        
    }
    public void EquipSecondaryWeapon(GameObject Weapon)
    {
        if (playerWeapons.Weapons[2].activeInHierarchy)
        {
            playerWeapons.Weapons[2] = Weapon;
            playerWeapons.CurrentWeapon.SetActive(false);
            playerWeapons.CurrentWeapon = Weapon;
            Weapon.SetActive(true);
        }
        else
        {
            playerWeapons.Weapons[2] = Weapon;
        }
    }
    public void LoadGame(PlayerData data)
    {
        Owned = data.Owned;
    }
}
