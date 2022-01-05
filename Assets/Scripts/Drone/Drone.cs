using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public int Health;
    private Animator DroneAnimator;
    private bool Destroyed;
    public GameObject Explosion;
    public GameObject BodyMesh;
    private AudioSource DroneAudioSource;
    public AudioClip FlyingSound;
    public AudioClip ExplosionSound;

    private void Start()
    {
        DroneAnimator = GetComponent<Animator>();
        DroneAudioSource = GetComponent<AudioSource>();
        DroneAudioSource.clip = FlyingSound;
        DroneAudioSource.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Destroyed == false)
        {
            if (collision.gameObject.GetComponent<Bullet>())
            {
                if (collision.gameObject.tag == "PlayerBullet")
                    Health --;
                else if (collision.gameObject.tag == "SMG")
                    Health --;
                else if (collision.gameObject.tag == "AR")
                    Health --;
                else if (collision.gameObject.tag == "Shotgun")
                    Health -= 15;
                else if (collision.gameObject.tag == "Sniper")
                    Health -= 20;
                if (Health <= 0)
                {
                    Destroyed = true;
                    DroneAnimator.SetBool("Destroy", true);
                }
                else
                {
                    DroneAnimator.SetTrigger("Hit");
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Destroyed == false)
        {
            if (other.gameObject.tag == "PlayerBullet")
            {
                Health--;
                if (Health <= 0)
                {
                    Destroyed = true;
                    DroneAnimator.SetBool("Destroy", true);
                }
                else
                {
                    DroneAnimator.SetTrigger("Hit");
                }
            }
        }
    }
    public void ExplodeDrone()
    {
        Explosion.SetActive(true);
        DroneAudioSource.Stop();
        DroneAudioSource.PlayOneShot(ExplosionSound);
        BodyMesh.SetActive(false);
        Invoke("TurnOffDrone", 5f);

    }
    void TurnOffDrone()
    {
        transform.gameObject.SetActive(false);
    }
}
