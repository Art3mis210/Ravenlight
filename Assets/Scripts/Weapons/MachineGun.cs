using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapons
{
    public GameObject Bullet;
    public GameObject Muzzle;
    private bool CanFire;
    public float FireRate = 0.1f;
    private void Start()
    {
        CanFire = true;
    }
    public override float Shoot()
    {
        if (CanFire == true)
        {
            CanFire = false;
            StartCoroutine(Fire());
        }
        return 0;
    }

    IEnumerator Fire()
    {
        GameObject newBullet1 = (GameObject)Instantiate(Bullet, Muzzle.transform.position + 0.1f * Muzzle.transform.right, Muzzle.transform.rotation);
        newBullet1.GetComponent<Rigidbody>().AddForce(1000 * newBullet1.transform.forward);
        GameObject newBullet2 = (GameObject)Instantiate(Bullet, Muzzle.transform.position - 0.1f * Muzzle.transform.right, Muzzle.transform.rotation);
        newBullet2.GetComponent<Rigidbody>().AddForce(1000 * newBullet2.transform.forward);
        GameObject newBullet3 = (GameObject)Instantiate(Bullet, Muzzle.transform.position + 0.1f * Muzzle.transform.up, Muzzle.transform.rotation);
        newBullet3.GetComponent<Rigidbody>().AddForce(1000 * newBullet3.transform.forward);
        yield return new WaitForSeconds(FireRate);
        CanFire = true;
    }
}
