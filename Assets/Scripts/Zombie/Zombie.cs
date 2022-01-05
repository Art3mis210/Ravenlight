using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private int ZombieBehaviour;
    private Animator ZombieAnimator;
    private NavMeshAgent Agent;
    public GameObject Target;
    private int WanderORIdle;
    private float TimeForWanderORIdle;
    private bool ChangeEvent;
    public BoxCollider Head;
    public BoxCollider LeftHand;
    public BoxCollider RightHand;
    public ZombieSpawner Spawner;
    public int Health;
    private AudioSource ZombieAudioSource;
    void Start()
    {
        ZombieBehaviour = Random.Range(0, 5);
        ZombieAnimator = GetComponent<Animator>();
        ZombieAnimator.SetFloat("Behaviour", ZombieBehaviour);
        Agent = GetComponent<NavMeshAgent>();
        ZombieAnimator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
        Ragdoll(false);
        ChangeEvent = true;
        ZombieAudioSource = GetComponent<AudioSource>();
        ZombieAudioSource.enabled = true;
    }
    private void Ragdoll(bool Status)
    {
        Rigidbody[] rigidBodies;
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = !Status;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ZombieAnimator.enabled)
        {
            if (!LineOfSight())
            {
                if (ChangeEvent)
                {
                    ChangeEvent = false;
                    WanderORIdle = Random.Range(0, 2);
                    TimeForWanderORIdle = Random.Range(100f, 200f);
                }
                if (ChangeEvent == false)
                {
                    TimeForWanderORIdle -= Time.deltaTime;
                    if (TimeForWanderORIdle < 0)
                        ChangeEvent = true;
                    if (WanderORIdle == 0)
                        Wander();
                    else
                        Idle();
                }
            }
        }
    }
    void Walk()
    {
        ZombieAnimator.SetBool("Walk", true);
        ZombieAnimator.SetBool("Attack", false);
        ZombieAnimator.SetBool("Chase", false);

    }
    void Chase()
    {
        ZombieAnimator.SetBool("Walk", false);
        ZombieAnimator.SetBool("Attack", false);
        ZombieAnimator.SetBool("Chase", true);
    }
    void Attack()
    {
        ZombieAnimator.SetBool("Walk", false);
        ZombieAnimator.SetBool("Attack", true);
        ZombieAnimator.SetBool("Chase", false);
    }
    void Idle()
    {
        Agent.ResetPath();
        ZombieAnimator.SetBool("Walk", false);
        ZombieAnimator.SetBool("Attack", false);
        ZombieAnimator.SetBool("Chase", false);
    }
    float visDistance = 100f;
    float visAngle = 150f;
    float ChaseDist = 80f;
    float AttackDist = 0.75f;
    Vector3 direction;
    bool LineOfSight()
    {
        if (Target != null)
        {
            direction = Target.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            if (direction.magnitude < visDistance && angle < visAngle)
            {
                Agent.SetDestination(Target.transform.position);
                if (direction.magnitude < AttackDist)
                {
                    Attack();
                }
                else if (direction.magnitude < ChaseDist)
                {
                    Chase();
                }
                else
                {
                    Walk();
                }
                return true;
            }
        }
        return false;
    }
    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderDist = 10;
        float wanderRadius = 10;
        float wanderJitter = 1;
        wanderTarget += new Vector3(Random.Range(-5.0f, 5.0f) * wanderJitter, transform.position.y, Random.Range(-5f, 5f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDist);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(targetWorld, out hit, 1f, NavMesh.AllAreas))
        {
            break;
        }
        Agent.SetDestination(hit.position);
        Walk();
        Debug.DrawRay(hit.position, Vector3.down, Color.blue, 1.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() && ZombieAnimator.enabled)
        {
            if (Health > 0)
            {
                if (collision.gameObject.tag == "PlayerBullet")
                    Health -= 3;
                if (collision.gameObject.tag == "EnemyBullet")
                    Health -= 3;
                else if (collision.gameObject.tag == "SMG")
                    Health -= 4;
                else if (collision.gameObject.tag == "AR")
                    Health -= 5;
                else if (collision.gameObject.tag == "Shotgun")
                    Health -= 15;
                else if (collision.gameObject.tag == "Sniper")
                    Health -= 20;
            }
            else
            {
                transform.GetComponent<Collider>().enabled = false;
                Agent.enabled = false;
                ZombieAnimator.enabled = false;
                Ragdoll(true);
                Head.enabled = false;
                LeftHand.enabled = false;
                RightHand.enabled = false;
                ZombieAudioSource.enabled = false;
                Invoke("DestroyZombie", 10f);
            }
        }
    }
    private void DestroyZombie()
    {
        if(Spawner!=null)
            Spawner.StartRespawn(this);
    }
    private void OnEnable()
    {
        Start();
        Head.enabled = true;
        LeftHand.enabled = true;
        RightHand.enabled = true;
    }
}
