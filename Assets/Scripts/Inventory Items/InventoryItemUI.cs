using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Inventory playerInventory;
    private Text InventoryText;
    public int InventoryItemNo;
    void Start()
    {
        InventoryText = GetComponent<Text>();
    }
    void Update()
    {
        InventoryText.text = playerInventory.InventoryItems[InventoryItemNo].ToString() + " / " + playerInventory.InventoryItems[InventoryItemNo + 1].ToString();
    }
}
