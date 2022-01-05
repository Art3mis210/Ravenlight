using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public virtual float Shoot()
    {
        return 0;
    }
    public virtual int ReloadWeapon(int RemainingBullets)
    {
        return 0;
    }
    public virtual int GetBullets()
    {
        return 0;
    }
    public virtual void ReloadSound()
    {

    }
}
