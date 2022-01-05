using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelSpriteChanger : MonoBehaviour
{
    public PlayerWeapons playerWeapons;
    public List<Sprite> PrimaryWeaponSprites;
    public List<Sprite> SecondaryWeaponSprites;
    public List<string> PrimaryWeaponName;
    public List<string> SecondaryWeaponName;
    public Image PrimaryWeaponWheelSprite;
    public Image SecondaryWeaponWheelSprite;
    public Text PrimaryWeaponText;
    public Text SecondaryWeaponText;
    private void OnEnable()
    {
        for (int i = 0; i < playerWeapons.PrimaryWeapons.Count; i++)
        {
            if (playerWeapons.Weapons[1] == playerWeapons.PrimaryWeapons[i])
            {
                PrimaryWeaponWheelSprite.sprite = PrimaryWeaponSprites[i];
                PrimaryWeaponText.text= PrimaryWeaponName[i];
                break;
            }
        }
        for (int i = 0; i < playerWeapons.SecondaryWeapons.Count; i++)
        {
            if (playerWeapons.Weapons[2] == playerWeapons.SecondaryWeapons[i])
            {
                SecondaryWeaponWheelSprite.sprite = SecondaryWeaponSprites[i];
                SecondaryWeaponText.text = SecondaryWeaponName[i];
                break;
            }
        }
    }

}
