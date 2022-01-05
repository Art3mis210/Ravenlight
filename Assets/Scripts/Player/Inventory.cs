using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private PlayerWeapons playerWeapons;
    private Animator InventoryAnimator;
    public GameManager gameManager;

    public GameObject Wheel;

    public Image HealthMeter;
    public Image StaminaMeter;
    public Image FocusMeter;

    public GameObject WeaponsDescription;
    public GameObject BagItemsDescription;

    public int[] InventoryItems;
    public int Ark;

    bool WheelStatus;

    //01 health 23 stamina 45 arammo 67 pistolammo 
    //89 medkit 1011 water  1213 canned food   1415 energydrink  1617 herbs  1819ammobox 2021 miracle  2223 focusmeter
    public void LoadGame(PlayerData data)
    {
        for (int i = 0; i < 24; i++)
            InventoryItems[i] = data.InventoryItems[i];
        Ark = data.Ark;
    }
    private void Start()
    {
        playerWeapons = GetComponent<PlayerWeapons>();
        InventoryAnimator = Wheel.GetComponent<Animator>();
        WheelStatus = false;
    }
    // Update is called once per frame
    void Update()
    {
        HealthMeter.fillAmount = InventoryItems[0] / 100.0f;
        StaminaMeter.fillAmount = InventoryItems[2] / 100.0f;
        FocusMeter.fillAmount= InventoryItems[22] / 100.0f;
        if (Input.GetKey(KeyCode.Tab) && WheelStatus==false && Time.timeScale==1)
        {
            WheelStatus = true;
            Wheel.SetActive(true);
            Time.timeScale = 0.25f;
        }
        else if(!Input.GetKey(KeyCode.Tab) && WheelStatus==true && Time.timeScale ==0.25f)
        {
            WheelStatus = false;
            Wheel.SetActive(false);
            TurnOffWheel();
            Time.timeScale = 1;
        }
    }
    private void TurnOffWheel()
    {
        
    }
    public void Fist()
    {
        playerWeapons.ChangeWeapon(0);
    }
    public void PrimaryWeapon()
    {
        playerWeapons.ChangeWeapon(1);
    }
    public void SecondaryWeapon()
    {
        playerWeapons.ChangeWeapon(2);
    }
    public void AmmoBoxOpened()
    {
        if(InventoryItems[18]>0)
        {
            InventoryItems[18]--;
            InventoryItems[4] = InventoryItems[5];
            InventoryItems[6] = InventoryItems[7];
        }
    }
    public void FirstAidTaken()
    {
        if (InventoryItems[8] > 0 && InventoryItems[0] != InventoryItems[1])
        {
            InventoryItems[0] = InventoryItems[1];
            InventoryItems[8]--;
        }
    }
    public void CannedFoodTaken()
    {
        if (InventoryItems[12] > 0 && (InventoryItems[0] != InventoryItems[1] || InventoryItems[2] != InventoryItems[3]))
        {
            int StaminaToBeIncreased = InventoryItems[3] / 4;
            InventoryItems[2] = InventoryItems[2] + StaminaToBeIncreased > InventoryItems[3] ? InventoryItems[3] : InventoryItems[2] + StaminaToBeIncreased;
            int HealthToBeAdded = InventoryItems[1] / 4;
            InventoryItems[0] = (InventoryItems[0] + HealthToBeAdded) > InventoryItems[1] ? InventoryItems[1] : InventoryItems[0] + HealthToBeAdded;
            InventoryItems[12]--;

        }
    }

    public void EnergyDrinkTaken()
    {
        if (InventoryItems[14] > 0)
        {
            InventoryItems[2] = InventoryItems[3];
            InventoryItems[14]--;
        }
    }
    public void WaterTaken()
    {
        if (InventoryItems[10] > 0 && InventoryItems[2] != InventoryItems[3])
        {
            int StaminaToBeIncreased = InventoryItems[3] / 4;
            InventoryItems[2] = InventoryItems[2]+ StaminaToBeIncreased> InventoryItems[3] ? InventoryItems[3] : InventoryItems[2] + StaminaToBeIncreased;
            InventoryItems[10]--;
        }
    }
    public void MiracleTonicTaken()
    {
        if (InventoryItems[20] > 0 && (InventoryItems[0] != InventoryItems[1] || InventoryItems[2] != InventoryItems[3] || InventoryItems[22] != InventoryItems[23]))
        {
            InventoryItems[0] = InventoryItems[1];
            InventoryItems[2] = InventoryItems[3];
            InventoryItems[22] = InventoryItems[23];
            InventoryItems[20]--;
        }
    }
    public void HerbsTaken()
    {
        if (InventoryItems[16] > 0 && (InventoryItems[0] != InventoryItems[1] || InventoryItems[22] != InventoryItems[23]))
        {
            int FocusToBeIncreased = InventoryItems[23] / 4;
            InventoryItems[22] = InventoryItems[22] + FocusToBeIncreased > InventoryItems[23] ? InventoryItems[23] : InventoryItems[22] + FocusToBeIncreased;
            int HealthToBeAdded = InventoryItems[1] / 4;
            InventoryItems[0] = (InventoryItems[0] + HealthToBeAdded) > InventoryItems[1] ? InventoryItems[1] : InventoryItems[0] + HealthToBeAdded;
            InventoryItems[16]--;
        }
    }
    public void Bag()
    {
        InventoryAnimator.SetBool("Bag", true);
    }
    public void Weapons()
    {
        InventoryAnimator.SetBool("Bag", false);
    }
}
