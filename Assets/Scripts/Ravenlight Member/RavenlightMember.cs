using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RavenlightMember : MonoBehaviour
{
    public enum CompanionBehaviour
    {
        FollowPlayer = 1, HideFromPlayer = 2, FightWithPlayer = 3, StandIdle = 4, GoTowardsTarget = 5, GoAwayFromPlayer = 6, ShootPlayerOnSight = 7, FindAndHuntPlayer = 8,CompanionMode = 9,FollowCompanionMode = 10
    }
    public CompanionBehaviour companionBehaviour;
    private NavMeshAgent agent;
    private Animator RavenlightAnimator;
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

    public List<GameObject> Targets;
    private int CurrentTarget;
    void Start()
    {
        Ragdoll(false);
        agent = GetComponent<NavMeshAgent>();
        RavenlightAnimator = GetComponent<Animator>();
        Health = 25;
        StateVis.color = Color.green;
        CurrentTarget = 0;
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

    void LateUpdate()
    {
        if (RavenlightAnimator.enabled)
        {
            if (companionBehaviour == CompanionBehaviour.FollowPlayer)
            {
                Seek(Player.transform.position);
                FollowPlayer();
            }
            else if (companionBehaviour == CompanionBehaviour.HideFromPlayer)
            {
                CanSeeTarget();
            }

            else if (companionBehaviour == CompanionBehaviour.StandIdle)
            {
                StopMoving();
            }
            else if (companionBehaviour == CompanionBehaviour.GoTowardsTarget)
            {
                Seek(Target.position);
                StartMoving();
            }
            else if (companionBehaviour == CompanionBehaviour.GoAwayFromPlayer)
            {
                Flee(Player.transform.position);
                StartMoving();
            }
            else if (companionBehaviour == CompanionBehaviour.ShootPlayerOnSight)
            {
                LineOfSight();
            }
            else if (companionBehaviour == CompanionBehaviour.FightWithPlayer)
            {
                ShootPlayer(Player.transform);
            }
            else if (companionBehaviour == CompanionBehaviour.FindAndHuntPlayer)
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
            else if(companionBehaviour == CompanionBehaviour.CompanionMode)
            {
                ShootingCompanion();
            }
            else if(companionBehaviour == CompanionBehaviour.FollowCompanionMode)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) > 10f)
                {
                    Seek(Player.transform.position);
                    FollowPlayer();
                    
                }
                else
                {
                    ShootingCompanion();
                }
            }
            
        }

    }
    void FollowPlayer()
    {
        
        if (Vector3.Distance(transform.position,Player.transform.position)>15f)
        {
            agent.speed = 0.1f;
            RavenlightAnimator.SetFloat("MoveSpeed", 1);
            RavenlightAnimator.SetBool("MoveForward", true);
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) > 5f)
        {
            agent.speed = 0.1f;
            RavenlightAnimator.SetFloat("MoveSpeed", 0);
            RavenlightAnimator.SetBool("MoveForward", true);
        }
        else
        {
            agent.speed = 0;
            RavenlightAnimator.SetFloat("MoveSpeed", 0);
            RavenlightAnimator.SetBool("MoveForward", false);
        }
    }
    void ShootingCompanion()
    {
        agent.speed = 0;
        RavenlightAnimator.SetFloat("MoveSpeed", 0);
        RavenlightAnimator.SetBool("MoveForward", false);
        if (Targets.Count > 0)
        {
            if (Targets[CurrentTarget].GetComponent<Animator>().enabled)
            {
                if (Vector3.Distance(Targets[CurrentTarget].transform.position, transform.position) < 20f)
                {
                    ShootPlayer(Targets[CurrentTarget].transform);
                }
                else if (Vector3.Distance(Targets[CurrentTarget].transform.position, transform.position) > 50f)
                {
                    Targets.RemoveAt(CurrentTarget);
                }
                else
                {
                    CurrentTarget = (CurrentTarget + 1) % Targets.Count;
                }
            }
            else
            {
                Targets.RemoveAt(CurrentTarget);
                if (Targets.Count > 0)
                    CurrentTarget = (CurrentTarget + 1) % Targets.Count;
                else
                {
                    RavenlightAnimator.SetBool("Aim", false);
                }
            }
        }
        else if (Targets.Count <= 0)
        {
            RavenlightAnimator.SetBool("Aim", false);
        }
    }
    void StartMoving()
    {
        if (agent.destination != null && Target.position != Vector3.zero)
        {
            if (Vector3.Distance(Target.position, transform.position) <= 0.5f)
            {
                RavenlightAnimator.SetBool("MoveForward", false);
                StopMoving();
            }
            else
            {
                agent.speed = 0.1f;
                RavenlightAnimator.SetBool("MoveForward", true);
                if (Vector3.Distance(Target.position, transform.position) > 5 * agent.stoppingDistance)
                {
                    if (RavenlightAnimator.GetFloat("MoveSpeed") < 1f)
                    {
                        RavenlightAnimator.SetFloat("MoveSpeed", RavenlightAnimator.GetFloat("MoveSpeed") + Time.deltaTime);
                        //agent.speed = 5;
                    }
                }
                else
                {
                    if (RavenlightAnimator.GetFloat("MoveSpeed") > 0f)
                    {
                        RavenlightAnimator.SetFloat("MoveSpeed", RavenlightAnimator.GetFloat("MoveSpeed") - Time.deltaTime);
                        //agent.speed = 2;
                    }
                }
            }
        }
        else
        {
            RavenlightAnimator.SetBool("MoveForward", false);
        }
    }
    void StopMoving()
    {
        agent.speed = 0;
        RavenlightAnimator.SetBool("MoveForward", false);
        RavenlightAnimator.SetFloat("MoveSpeed", 0);
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
        if (collision.gameObject.tag == "EnemyBullet")
        {
            if (Health <= 0)
            {
                RavenlightAnimator.enabled = false;
                agent.enabled = false;
                Ragdoll(true);
                transform.GetComponent<Collider>().enabled = false;
            }
            else
            {
                Health--;
            }
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
        RavenlightAnimator.SetBool("MoveForward", false);
        Vector3 TargetPos = new Vector3(TargetToShoot.transform.position.x, transform.position.y, TargetToShoot.transform.position.z);
        transform.LookAt(TargetPos);
        RavenlightAnimator.SetFloat("Weapon", 1);
        RavenlightAnimator.SetBool("Aim", true);
        Transform Chest;
        if (AnimatorChestSpine == 0)
            Chest = RavenlightAnimator.GetBoneTransform(HumanBodyBones.Spine);
        else
            Chest = RavenlightAnimator.GetBoneTransform(HumanBodyBones.Chest);
        Chest.LookAt(TargetToShoot.position);
        Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        float Reload = Gun.GetComponent<Weapons>().Shoot();
        if (Reload == -1 && Reloading == false)
        {
            Reloading = true;
            RavenlightAnimator.SetBool("Reload", true);
            RavenlightAnimator.SetInteger("ReloadDir", 0);
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
                StateVis.color = Color.yellow;
            }
            else
            {
                State = "Shooting";
                ShootPlayer(Player.transform);
                StateVis.color = Color.green;
            }
        }
        else
        {
            State = "Idle";
            StateVis.color = Color.green;
            StopMoving();

        }

    }
    private void Move()
    {
        RavenlightAnimator.SetFloat("Weapon", 1);
        RavenlightAnimator.SetBool("Aim", false);
        agent.speed = 2;
        RavenlightAnimator.SetBool("MoveForward", true);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }
    public void TurnAnimationoff(string AnimationName)
    {
        RavenlightAnimator.SetBool(AnimationName, false);
    }
   

}

