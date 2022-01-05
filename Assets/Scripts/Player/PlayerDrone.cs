using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
{
    private Animator DroneAnimator;
    public GameObject Flame;
    private MachineGun Gun;
    public AudioSource GunAudioSource;
    private bool Firing;
    public AudioClip WindUp;
    public AudioClip WindDown;
    public AudioClip FiringSound;

    void Start()
    {
        DroneAnimator = GetComponent<Animator>();
        Gun=GetComponent<MachineGun>();
        Firing = false;
    }
    void Update()
    {
        Movement();
        Attack();
    }
    void Movement()
    {
        DroneAnimator.SetBool("Forward", Input.GetAxis("Vertical") > 0);
        DroneAnimator.SetBool("Backward", Input.GetAxis("Vertical") < 0);
        DroneAnimator.SetBool("Right", Input.GetAxis("Horizontal") > 0);
        DroneAnimator.SetBool("Left", Input.GetAxis("Horizontal") < 0);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(10*transform.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(-10 * transform.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(transform.up, -60 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(transform.up, 60 * Time.deltaTime);
        }
    }
    void Attack()
    {
        if(Input.GetKey(KeyCode.Alpha0))
        {
            DroneAnimator.SetInteger("Axis", 0);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            DroneAnimator.SetInteger("Axis", 1);
        }
        if (Input.GetMouseButton(0))
        {
            if(Firing==false)
            {
                GunAudioSource.clip = WindUp;
                GunAudioSource.Play();
                Invoke("ChangeGunSound", 0.5f);
                Firing = true;
            }
            DroneAnimator.SetBool("Minigun", true);
            Gun.Shoot();
        }
        else
        {
            if (Firing == true)
            {
                GunAudioSource.loop = false;
                GunAudioSource.clip = WindDown;
                GunAudioSource.Play();
                Firing = false;

            }
            DroneAnimator.SetBool("Minigun", false);
        }
        
        DroneAnimator.SetBool("Fire", Input.GetMouseButton(1));
        if(Input.GetMouseButton(1))
        {
            DroneAnimator.SetBool("Fire", true);
            Flame.SetActive(true);
        }
        else
        {
            DroneAnimator.SetBool("Fire", false);
            Flame.SetActive(false);
        }

    }
    void ChangeGunSound()
    {
        GunAudioSource.clip = FiringSound;
        GunAudioSource.loop = true;
        GunAudioSource.Play();
    }
}
