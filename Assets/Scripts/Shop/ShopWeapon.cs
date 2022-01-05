using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeapon : MonoBehaviour
{
    public GameObject Buy;
    public GameObject Equip;
    public Shop shop;
    public int WeaponAddress;
    // Update is called once per frame
    void OnEnable()
    {
        if(shop.Owned[WeaponAddress]==true)
        {
            Buy.SetActive(false);
            Equip.SetActive(true);
        }
        else
        {
            Buy.SetActive(true);
            Equip.SetActive(false);
        }
    }
    public void BuyWeapon(int Ark)
    {
        if (shop.playerInventory.Ark >= Ark)
        {
            shop.playerInventory.Ark -= Ark;
            shop.Owned[WeaponAddress] = true;
            Buy.SetActive(false);
            Equip.SetActive(true);
        }
    }
    public void EquipPrimaryWeapon()
    {
        shop.EquipPrimaryWeapon(shop.WeaponsOnPlayer[WeaponAddress]);

    }
    public void EquipSecondaryWeapon()
    {
        shop.EquipSecondaryWeapon(shop.WeaponsOnPlayer[WeaponAddress]);
    }
}
