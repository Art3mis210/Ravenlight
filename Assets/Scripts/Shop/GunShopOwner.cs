using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShopOwner : MonoBehaviour
{
    public Shop shop;
    public Text EnterShop;
    public GameManager gameManager;
    public GameObject InteractionIndicator;
    public void Update()
    {
        if(gameManager.PlayerBike.OnBike==true && shop.InShop==true)
        {
            shop.InShop = false;
            gameManager.EnableCursor = false;
            InteractionIndicator.SetActive(false);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {   
            if (shop.InShop == false)
            {
                InteractionIndicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    shop.InShop = true;
                    gameManager.EnableCursor = true;
                    InteractionIndicator.SetActive(false);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    shop.InShop = false;
                    gameManager.EnableCursor = false;
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            InteractionIndicator.SetActive(false);
            shop.InShop = false;
            gameManager.EnableCursor = false;
        }
    }
}
