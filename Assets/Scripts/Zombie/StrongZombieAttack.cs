using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongZombieAttack : MonoBehaviour
{
    public StrongZombie strongZombie;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Player" && strongZombie.Attacking)
        {
            strongZombie.KnockPlayerOff();
        }
    }
}
