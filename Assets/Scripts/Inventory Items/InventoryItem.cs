using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public int ItemNo;
    public AudioClip PickSound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Inventory>())
        {
            Inventory PlayerInventory = other.gameObject.GetComponent<Inventory>();
            if(PlayerInventory.InventoryItems[ItemNo]< PlayerInventory.InventoryItems[ItemNo+1])
            {
                PlayerInventory.InventoryItems[ItemNo]++;
                other.gameObject.GetComponent<AudioSource>().PlayOneShot(PickSound);
                Destroy(gameObject);
            }
        }
    }
}
