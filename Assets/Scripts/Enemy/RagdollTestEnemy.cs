using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RagdollTestEnemy : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        FollowPlayer = 1, HideFromPlayer = 2, FightWithPlayer = 3, StandIdle = 4, GoTowardsTarget = 5, GoAwayFromPlayer = 6, ShootPlayerOnSight = 7, FindAndHuntPlayer = 8
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
    private bool Reloading;
    public int AnimatorChestSpine;
    public EnemySpawner Spawner;
    private bool RagdollMode;
    public RuntimeAnimatorController RagdollController;
    void Start()
    {
        //Ragdoll(false);
        agent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<Animator>();
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
                    StartMoving();
                }
            }
        }

    }
    void StartMoving()
    {
        EnemyAnimator.SetBool("Aim", false);
        if (agent.destination != null && Target.position != Vector3.zero)
        {
            if (Vector3.Distance(Target.position, transform.position) <= agent.stoppingDistance)
            {
                EnemyAnimator.SetBool("MoveForward", false);
                StopMoving();
            }
            else
            {
                EnemyAnimator.SetBool("MoveForward", true);
                if (Vector3.Distance(Target.position, transform.position) > 5 * agent.stoppingDistance)
                {
                    if (EnemyAnimator.GetFloat("MoveSpeed") < 1f)
                    {
                        EnemyAnimator.SetFloat("MoveSpeed", EnemyAnimator.GetFloat("MoveSpeed") + Time.deltaTime);
                        //agent.speed = 5;
                    }
                }
                else
                {
                    if (EnemyAnimator.GetFloat("MoveSpeed") > 0f)
                    {
                        EnemyAnimator.SetFloat("MoveSpeed", EnemyAnimator.GetFloat("MoveSpeed") - Time.deltaTime);
                        //agent.speed = 2;
                    }
                }
            }
        }
        else
        {
            EnemyAnimator.SetBool("MoveForward", false);
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
        Debug.DrawRay(this.transform.position + this.transform.up, rayToTarget, Color.red, 2f);
        if (Physics.Raycast(this.transform.position + this.transform.up, 2 * rayToTarget, out raycastInfo, Mathf.Infinity))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
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
        if (collision.gameObject.tag == "PlayerBullet" && RagdollMode == false)
        {
            transform.GetComponent<Collider>().enabled = false;
            EnemyAnimator.enabled = false;
            RagdollMode = true;
            Ragdoll(true);
            if(Health>0)
                Invoke("GetUp", 5f);
        }
    }
    private void GetUp()
    {
        if(Health>0 && RagdollMode==true)
        {
            RagdollMode = false;
            Ragdoll(false);
            EnemyAnimator.runtimeAnimatorController = RagdollController;
            EnemyAnimator.enabled = true;
            transform.GetComponent<Collider>().enabled = true;
        }
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
        Target.position = location;
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
        if (AnimatorChestSpine == 0)
            Chest = EnemyAnimator.GetBoneTransform(HumanBodyBones.Spine);
        else
            Chest = EnemyAnimator.GetBoneTransform(HumanBodyBones.Chest);
        Chest.LookAt(TargetToShoot.position);
        Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        float Reload = Gun.GetComponent<Weapons>().Shoot();
        if (Reload == -1 && Reloading == false)
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
    
}
