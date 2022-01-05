using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        FollowPlayer = 1, HideFromPlayer = 2, FightWithPlayer = 3 , StandIdle = 4 , GoTowardsTarget = 5 , GoAwayFromPlayer = 6, ShootPlayerOnSight = 7, FindAndHuntPlayer = 8, Wander = 9
    }
    public EnemyBehaviour enemyBehaviour;
    private NavMeshAgent agent;
    private Animator EnemyAnimator;
    public Transform Target;
    public float Health;
    public GameObject Player;
    public List<GameObject> CoverList;
    public GameObject Gun;
    public Vector3 Offset;
    public string State;
    public SpriteRenderer StateVis;
    private bool Reloading;
    public int AnimatorChestSpine;
    public EnemySpawner Spawner;

    private Vector3 startPos;
    private bool MovingTowardsTarget;
    private Vector3 WanderTarget;

    public Vector3 GunWalkPos;
    private Vector3 GunShootPos;

    private AudioSource EnemyAudio;
    public AudioClip DeathSound;
    void Start()
    {
        Ragdoll(false);
        agent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<Animator>();
        StateVis.color = Color.red;
        startPos = transform.position;
        MovingTowardsTarget = false;
        GunShootPos = Gun.transform.localPosition;
        EnemyAudio = GetComponent<AudioSource>();
        EnemyAudio.pitch = Random.Range(1.0f, 3.0f);
    }
    private void OnEnable()
    {
        Start();
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

    // Update is called once per frame
    private void Update()
    {
        if(EnemyAnimator.GetBool("MoveForward")==true)
        {
            Gun.transform.localPosition = GunWalkPos;
        }
        else 
        {
            Gun.transform.localPosition = GunShootPos;
        }
        if (Vector3.Distance(Player.transform.position, transform.position) < 10f && enemyBehaviour != EnemyBehaviour.FindAndHuntPlayer)
            enemyBehaviour = EnemyBehaviour.ShootPlayerOnSight;
    }
    void LateUpdate()
    {
        if (EnemyAnimator.enabled)
        {
            if (enemyBehaviour == EnemyBehaviour.FollowPlayer)
            {
                Seek(Player.transform.position);
                StartMoving();
                //agent.stoppingDistance = 2f;
            }
            else if (enemyBehaviour == EnemyBehaviour.HideFromPlayer)
            {
                CanSeeTarget();
            }

            else if (enemyBehaviour == EnemyBehaviour.StandIdle)
            {
                StopMoving();
            }
            else if (enemyBehaviour == EnemyBehaviour.GoTowardsTarget)
            {
                Seek(Target.position);
                StartMoving();
            }
            else if (enemyBehaviour == EnemyBehaviour.GoAwayFromPlayer)
            {
                Flee(Player.transform.position);
                StartMoving();
            }
            else if (enemyBehaviour == EnemyBehaviour.ShootPlayerOnSight)
            {
                LineOfSight();
            }
            else if (enemyBehaviour == EnemyBehaviour.FightWithPlayer)
            {
                ShootPlayer(Player.transform);
            }
            else if (enemyBehaviour == EnemyBehaviour.FindAndHuntPlayer)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < 10f)
                {
                    ShootPlayer(Player.transform);
                }
                else
                {
                    Seek(Player.transform.position);
                    Target = Player.transform;
                    StartMoving();
                }
            }
            else if (enemyBehaviour == EnemyBehaviour.Wander)
                Wander();
        }

    }
    void StartMoving()
    {
        EnemyAnimator.SetBool("Aim", false);
        if (agent.destination != null && Target.position != Vector3.zero)
        {
            if (Vector3.Distance(Target.position, transform.position) <= 10)
            {
                EnemyAnimator.SetBool("MoveForward", false);
                StopMoving();
            }
            else
            {
                EnemyAnimator.SetBool("MoveForward", true);
                if (Vector3.Distance(Target.position, transform.position) >10)
                {
                    if (EnemyAnimator.GetFloat("MoveSpeed") < 1f)
                    {
                        EnemyAnimator.SetFloat("MoveSpeed", EnemyAnimator.GetFloat("MoveSpeed") + Time.deltaTime);
                        agent.speed = 0.1f;
                    }
                }
                else
                {
                    if (EnemyAnimator.GetFloat("MoveSpeed") > 0f)
                    {
                        EnemyAnimator.SetFloat("MoveSpeed", EnemyAnimator.GetFloat("MoveSpeed") - Time.deltaTime);
                        agent.speed = 0.1f;
                    }
                }
            }
        }
        else
        {
            EnemyAnimator.SetBool("MoveForward", false);
            MovingTowardsTarget = false;
        } 
    }
    void StopMoving()
    {
        agent.speed = 0;
        EnemyAnimator.SetBool("MoveForward", false);
        EnemyAnimator.SetFloat("MoveSpeed", 0);
        
    }
    void Hide()
    {
        float distance = Mathf.Infinity;
        Vector3 ChosenSpot = Vector3.zero;

        for (int i = 0; i < CoverList.Count; i++)
        {
            Vector3 hideDir = CoverList[i].transform.position - Player.transform.position;
            Vector3 hidePos = CoverList[i].transform.position + hideDir.normalized * 5;

            if (Vector3.Distance(this.transform.position, hidePos) < distance)
            {
                ChosenSpot = hidePos;
                distance = Vector3.Distance(this.transform.position, hidePos);
            }
        }
        Seek(ChosenSpot);
    }
    void CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = Player.transform.position - this.transform.position;
        Debug.DrawRay(this.transform.position+this.transform.up, rayToTarget, Color.red, 2f);
        if (Physics.Raycast(this.transform.position + this.transform.up, 2*rayToTarget, out raycastInfo,Mathf.Infinity))
        {
            if (raycastInfo.transform.gameObject.tag=="Player")
            {
                Hide();
                StartMoving();
                
            }
            else
            {
                Target.position = Vector3.zero;
                StopMoving();
            }
        }
        else
        {
            Target.position = Vector3.zero;
            StopMoving();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bullet>() && collision.gameObject.tag != "EnemyBullet")
        {
            if (Health <= 0)
            {
                EnemyAudio.PlayOneShot(DeathSound);
                EnemyAnimator.enabled = false;
                agent.enabled = false;
                Ragdoll(true);
                transform.GetComponent<Collider>().enabled = false;
                StateVis.enabled = false;
                Invoke("DestroyEnemy", 20f);
            }
            else
            {
                if (collision.gameObject.tag == "PlayerBullet")
                    Health-=5;
                else if (collision.gameObject.tag == "SMG")
                    Health -= 6;
                else if (collision.gameObject.tag == "AR")
                    Health -= 7;
                else if (collision.gameObject.tag == "Shotgun")
                    Health -= 15;
                else if (collision.gameObject.tag == "Sniper")
                    Health -= 20;
                Color ChangeColor = StateVis.color;
                ChangeColor.r = ChangeColor.r-Health / 100f;
                StateVis.color = ChangeColor;
            }
            if (enemyBehaviour != EnemyBehaviour.FindAndHuntPlayer)
                enemyBehaviour = EnemyBehaviour.FindAndHuntPlayer;
        }
    }
    void DestroyEnemy()
    {
        if(Spawner!=null)
        {
            gameObject.SetActive(false);
            Spawner.RespawnEnemy(this);
        }
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
        //Target.position = location;
    }
    void ShootPlayer(Transform TargetToShoot)
    {
        agent.speed = 0;
        EnemyAnimator.SetBool("MoveForward", false);
        Vector3 PlayerPos = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
        transform.LookAt(PlayerPos);
        EnemyAnimator.SetFloat("Weapon", 1);
        EnemyAnimator.SetBool("Aim", true);
        Transform Chest;
        if (AnimatorChestSpine==0)
            Chest = EnemyAnimator.GetBoneTransform(HumanBodyBones.Spine);
        else
            Chest = EnemyAnimator.GetBoneTransform(HumanBodyBones.Chest);
        Chest.LookAt(TargetToShoot.position);
        Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        float Reload=Gun.GetComponent<Weapons>().Shoot();
        if(Reload==-1 && Reloading==false)
        {
            Reloading = true;
            EnemyAnimator.SetBool("Reload", true);
            EnemyAnimator.SetInteger("ReloadDir", 0);
        }
    }
    public void Reload()
    {
        Gun.GetComponent<Weapons>().ReloadWeapon(60);
        Reloading = false;
    }
    void LineOfSight()
    {
        Vector3 direction = Player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        float visDistance = 30f;
        float visAngle = 150f;
        float shootDist = 20f;
        if (direction.magnitude < visDistance && angle < visAngle)
        {
            if (direction.magnitude > shootDist)
            {
                State = "Running";
                Seek(Player.transform.position);
                Move();
            }
            else
            {
                State = "Shooting";
                ShootPlayer(Player.transform);
            }
        }
        else
        {
            State = "Idle";
            StopMoving();

        }

    }
    private void Move()
    {
        EnemyAnimator.SetFloat("Weapon", 1);
        EnemyAnimator.SetBool("Aim", false);
        agent.speed = 2;
        EnemyAnimator.SetBool("MoveForward", true);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }
    public void TurnAnimationoff(string AnimationName)
    {
        EnemyAnimator.SetBool(AnimationName, false);
    }
    void Wander()
    {
        if(MovingTowardsTarget==false)
        {
            NavMeshHit hit;
            Vector3 WanderLocation = startPos + new Vector3(Random.Range(-25.0f, 25.0f), 0, Random.Range(-25f, 25f));
             while (!NavMesh.SamplePosition(WanderLocation, out hit, 1f, NavMesh.AllAreas))
             {
                 break;
             }
            MovingTowardsTarget = true;
            WanderTarget = WanderLocation;
            Seek(WanderLocation);
        }
        if (MovingTowardsTarget)
            WanderMove();
    }
    void WanderMove()
    {
        agent.speed = 0.1f;
        EnemyAnimator.SetFloat("MoveSpeed", 0f);
        if (Vector3.Distance(transform.position, WanderTarget) > 10f)
        {
            EnemyAnimator.SetBool("MoveForward", true);
        }
        else
        {
            EnemyAnimator.SetBool("MoveForward", false);
            MovingTowardsTarget = false;
        }
    }
}
