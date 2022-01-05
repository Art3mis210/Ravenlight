using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    public GameObject Bullet;
    public GameObject Muzzle;
    private bool CanFire;
    public float Recoil = 0.01f;
    public float FireRate = 0.1f;
    public int MaxBullets;
    private int Bullets;
    private bool Reload;
    private void Start()
    {
        CanFire = true;
        Reload = false;
        Bullets = MaxBullets;
    }
    public override float Shoot()
    {
        Debug.Log(Bullets);
        Debug.Log(Reload);

        if (CanFire == true && Reload == false)
        {
            CanFire = false;
            StartCoroutine(Fire());
            return Recoil;
        }
        return 0;
    }
    public override int ReloadWeapon(int RemainingBullets)
    {
        if (RemainingBullets > MaxBullets)
        {
            Bullets = MaxBullets;
            Reload = false;
            return RemainingBullets - MaxBullets;
        }
        else
        {
            Bullets = RemainingBullets;
            Reload = false;
            return 0;
        }

    }
    IEnumerator Fire()
    {
        GameObject newBullet = (GameObject)Instantiate(Bullet, Muzzle.transform.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
        if (Time.timeScale == 0.2f && Bullet.tag != "EnemyBullet")
            yield return new WaitForSeconds(FireRate * 0.2f);
        else
            yield return new WaitForSeconds(FireRate);
        CanFire = true;
        Bullets--;
        if (Bullets <= 0)
        {
            Reload = true;
        }
    }
    public override int GetBullets()
    {
        return Bullets;
    }
}



