using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StrongZombie : MonoBehaviour
{
    public GameObject Player;
    public int Health;
    private NavMeshAgent Agent;
    private Animator ZombieAnimator;
    public bool Attacking;
    private AudioSource StrongZombieAudioSource;
    private void Start()
    {
        Attacking = true;
        Agent = GetComponent<NavMeshAgent>();
        ZombieAnimator = GetComponent<Animator>();
        StrongZombieAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Health > 0)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) < 2f)
            {
                Agent.speed = 0;
                ZombieAnimator.SetBool("Walk", false);
                ZombieAnimator.SetBool("Sprint", false);
                ZombieAnimator.SetBool("Attack", true);
            }
            else 
            {
                Agent.SetDestination(Player.transform.position);
                Agent.speed = 5f;
                ZombieAnimator.SetBool("Walk", true);
                ZombieAnimator.SetBool("Sprint", true);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bullet>())
        {
            Health--;
            if (Health == 0)
            {
                Agent.speed = 0;
                ZombieAnimator.SetBool("Walk", false);
                ZombieAnimator.SetBool("Sprint", false);
                ZombieAnimator.SetBool("Death", true);
                Attacking = false;
                Invoke("TurnZombieOff", 5f);
            }
        }
    }
    public void AttackingState(int Status)
    {
        Attacking = Status == 1 ? true : false;
    }
    void TurnZombieOff()
    {
        transform.gameObject.SetActive(false);
    }
    public void KnockPlayerOff()
    {
        Player.GetComponent<Animator>().SetTrigger("FallBack");
    }
    public void PlayAudioOnce(AudioClip Clip)
    {
        StrongZombieAudioSource.PlayOneShot(Clip);
    }
}
