using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    
    public void OnButtonClick(GameObject On)
    {
        On.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
