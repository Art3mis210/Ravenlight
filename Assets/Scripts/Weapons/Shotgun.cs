using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapons
{
    public GameObject Bullet;
    public GameObject Muzzle;
    private bool CanFire;
    public float Recoil = 0.01f;
    public float FireRate = 0.1f;
    public int MaxBullets;
    private int Bullets;
    private bool Reload;
    public ParticleSystem MuzzleFlash;
    public AudioClip ShootSound;
    private AudioSource GunAudioSource;
    public AudioClip ReloadSoundClip;
    private void Start()
    {
        CanFire = true;
        Reload = false;
        Bullets = MaxBullets;
        GunAudioSource = GetComponent<AudioSource>();
    }
    public override float Shoot()
    {
        if (Reload == true)
            return -1;
        if (CanFire == true && Reload == false)
        {
            CanFire = false;
            StartCoroutine(Fire());
            return Recoil;
        }
        return 0;
    }
    public override void ReloadSound()
    {
        if (ReloadSoundClip != null)
            GunAudioSource.PlayOneShot(ReloadSoundClip);
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
        MuzzleFlash.Play();
        GunAudioSource.PlayOneShot(ShootSound);
        GameObject newBullet1 = (GameObject)Instantiate(Bullet, Muzzle.transform.position, transform.rotation);
        newBullet1.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
        GameObject newBullet2 = (GameObject)Instantiate(Bullet, Muzzle.transform.position+0.1f*Muzzle.transform.right, transform.rotation);
        newBullet2.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
        GameObject newBullet3 = (GameObject)Instantiate(Bullet, Muzzle.transform.position - 0.1f * Muzzle.transform.right, transform.rotation);
        newBullet3.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
        GameObject newBullet4 = (GameObject)Instantiate(Bullet, Muzzle.transform.position + 0.1f * Muzzle.transform.up, transform.rotation);
        newBullet4.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
        GameObject newBullet5 = (GameObject)Instantiate(Bullet, Muzzle.transform.position - 0.1f * Muzzle.transform.up, transform.rotation);
        newBullet5.GetComponent<Rigidbody>().AddForce(1000 * transform.forward);
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