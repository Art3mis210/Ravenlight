using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    public GameObject Item;
    private void OnEnable()
    {
        if(Item.activeInHierarchy)
            Item.SetActive(false);
    }
    public void TurnDescriptionOnOff(bool Status)
    {
        Item.SetActive(Status);
    }
}
