using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR : Weapons
{
    public GameObject Bullet;
    public GameObject Muzzle;
    private bool CanFire;
    public float Recoil = 0.01f;
    public float FireRate = 0.1f;
    public int MaxBullets;
    public int Bullets;
    private bool Reload;
    public ParticleSystem MuzzleFlash;
    public AudioClip ShootSound;
    private AudioSource GunAudioSource;
    public AudioClip ReloadSoundClip;
    //public GameObject MuzzleFlash;
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
        if(RemainingBullets>MaxBullets)
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
        GameObject newBullet = (GameObject)Instantiate(Bullet, Muzzle.transform.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(2000 * transform.forward);
        if(Time.timeScale==0.2f && Bullet.tag!="EnemyBullet")
            yield return new WaitForSeconds(FireRate*0.2f);
        else
            yield return new WaitForSeconds(FireRate);
        CanFire = true;
        Bullets--;
        if(Bullets<=0)
        {
            Reload = true;
        }
    }
    public override int GetBullets()
    {
        return Bullets;
    }
}



