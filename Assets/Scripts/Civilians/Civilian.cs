using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilian : MonoBehaviour
{
    public enum CivilianBehaviour
    {
        Happy=1 , Neutral=2 , Sad=3
    }
    public Transform Target;
    public CivilianBehaviour civilianBehaviour;
    private Animator CivilianAnimator;
    private NavMeshAgent Agent;
    public CivilianSpawner Spawner;
    public Animator PlayerAnimator;
    private bool EvadeMode;
    public AudioClip ScreamSound;
    private bool Scream;
    private AudioSource CivilianAudio;
    void Start()
    {
        Ragdoll(false);
        CivilianAnimator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        CivilianAnimator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
        EvadeMode = false;
        Scream = false;
        CivilianAudio = GetComponent<AudioSource>();
        CivilianAudio.pitch = Random.Range(1.0f, 3.0f);
    }
    private void OnEnable()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (CivilianAnimator.enabled)
        {
            if (civilianBehaviour == CivilianBehaviour.Happy)
            {
                CivilianAnimator.SetInteger("Behaviour", 1);
            }
            if (civilianBehaviour == CivilianBehaviour.Neutral)
            {
                CivilianAnimator.SetInteger("Behaviour", 0);
            }
            if (civilianBehaviour == CivilianBehaviour.Neutral)
            {
                CivilianAnimator.SetInteger("Behaviour", -1);
            }
            CheckPlayerBehaviour();
            if (!EvadeMode)
            {
                MoveTowardsTarget(); 
            }
            else
            {
                EvadePlayer();
            }
        }
    }
    void CheckPlayerBehaviour()
    {
        if(Vector3.Distance(transform.position,PlayerAnimator.transform.position)<30f)
        {
            if(PlayerAnimator.GetBool("Aim") && EvadeMode==false)
            {
                EvadeMode = true;
                CivilianAnimator.SetBool("Sprint", true);
                if(Scream==false)
                {
                    Scream = true;
                    GetComponent<AudioSource>().PlayOneShot(ScreamSound);
                }
            }
        }
        else if(Vector3.Distance(transform.position, PlayerAnimator.transform.position) > 100f)
        {
            EvadeMode = false;
            CivilianAnimator.SetBool("Sprint", false);
            if (Scream == true)
            {
                Scream = false;
            }
        }
    }
    void EvadePlayer()
    {
        Vector3 dirToPlayer = transform.position - PlayerAnimator.transform.position;
        Vector3 newPos = transform.position + dirToPlayer;
        Agent.SetDestination(newPos);
    }
    void MoveTowardsTarget()
    {
        Agent.SetDestination(Target.position);
        CivilianAnimator.SetBool("Walk", true);
    }
    void StopMoving()
    {
        CivilianAnimator.SetBool("Walk", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="TargetChanger")
        {
            Target=other.GetComponent<TargetChanger>().ChangeTarget();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bullet>() && CivilianAnimator.enabled)
        {
            transform.GetComponent<Collider>().enabled=false;
            Agent.enabled = false;
            CivilianAnimator.enabled = false;
            Ragdoll(true);
            Invoke("DestroyCivilian", 20f);
        }
    }
    private void DestroyCivilian()
    {
        gameObject.SetActive(false);
        Spawner.SpawnCivilian(this);

    }
    public void Ragdoll(bool Status)
    {
        Rigidbody[] rigidBodies;
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
           rigidBody.isKinematic = !Status;
        }
    }

}
