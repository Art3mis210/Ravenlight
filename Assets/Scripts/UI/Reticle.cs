using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reticle : MonoBehaviour
{
    RaycastHit hit;
    private Image ReticleImage;
    public PlayerWeapons playerWeapons;
    private void Start()
    {
        ReticleImage = GetComponent<Image>();
    }
    void Update()
    {
        if (playerWeapons.CurrentWeapon != null)
        {
            Debug.DrawRay(playerWeapons.CurrentWeapon.transform.position, playerWeapons.CurrentWeapon.transform.forward, Color.red);
            if (Physics.Raycast(playerWeapons.CurrentWeapon.transform.position, playerWeapons.CurrentWeapon.transform.forward, out hit, 500f))
            {
                if (hit.transform.gameObject.GetComponent<Enemy>())
                {
                    Color ReticleColor = Color.red;
                    ReticleColor.r = hit.transform.gameObject.GetComponent<Enemy>().Health / 100f;
                    ReticleImage.color = ReticleColor;
                }
                else if (hit.transform.gameObject.GetComponent<RavenlightMember>())
                {
                    ReticleImage.color = Color.green;
                }
                else if(hit.transform.gameObject.GetComponent<Zombie>())
                {
                    Color ReticleColor = Color.yellow;
                    ReticleColor.r = hit.transform.gameObject.GetComponent<Zombie>().Health / 100f;
                    ReticleColor.g = hit.transform.gameObject.GetComponent<Zombie>().Health / 100f;
                    ReticleImage.color = ReticleColor;
                }
                else
                    ReticleImage.color = Color.white;
            }
            else
                ReticleImage.color = Color.white;
        }
    }
}
