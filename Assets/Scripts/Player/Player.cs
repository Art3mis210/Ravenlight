using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    private CharacterController PlayerController;
    private Animator PlayerAnimator;
    private RaycastHit hit;
    private GameObject CurrentCover;
    private float CoverDir;
    private bool ChangePosition;
    private Vector3 PreviousPosition;
    private Quaternion PreviousRotation;
    private PlayerAim playerAim;
    private PlayerWeapons playerWeapons;
    private Inventory playerInventory;
    private bool ColliderStandorCover;
    private CapsuleCollider PlayerCollider;

    private bool StaminaDrain;
    private bool StaminaIncrease;

    private AudioSource PlayerAudioSource;
    private bool FocusMode;
    private bool FocusDecrease;
    private bool FocusIncrease;
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        ChangePosition = false;
        playerAim = GetComponent<PlayerAim>();
        playerWeapons = GetComponent<PlayerWeapons>();
        playerInventory = GetComponent<Inventory>();
        PlayerController = GetComponent<CharacterController>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        ColliderStandorCover = true;
        Ragdoll(false);
        StaminaDrain = true;
        StaminaIncrease = true;
        PlayerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (PlayerAnimator.enabled && playerInventory.InventoryItems[0]>0)
        {
            Movement();
            Cover();
            Aim();
            CheckIfFalling();
            DeadEye();
        }
        else
        {
            if (FocusMode == true)
            {
                FocusMode = false;
                Time.timeScale = 1;
                playerAim.PPP.GetSetting<ColorGrading>().saturation.value = 0;
            }
        }
    }
    void Movement()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            PlayerAnimator.SetBool("MoveForward", true);
            if (playerInventory.InventoryItems[2] > 0)
            {
                if (Input.GetAxis("Sprint") > 0 && PlayerAnimator.GetFloat("MoveSpeed") < 1)
                {
                    PlayerAnimator.SetFloat("MoveSpeed", PlayerAnimator.GetFloat("MoveSpeed") + 0.01f);
                }
                if (Input.GetAxis("Sprint") == 0 && PlayerAnimator.GetFloat("MoveSpeed") > 0)
                {
                    PlayerAnimator.SetFloat("MoveSpeed", PlayerAnimator.GetFloat("MoveSpeed") - 0.01f);
                }
            }
            else
            {
                if(PlayerAnimator.GetFloat("MoveSpeed") > 0)
                {
                    PlayerAnimator.SetFloat("MoveSpeed", PlayerAnimator.GetFloat("MoveSpeed") - 0.01f);
                }
            }
        }
        else
        {
            if (PlayerAnimator.GetFloat("MoveSpeed") > 0)
                PlayerAnimator.SetFloat("MoveSpeed", PlayerAnimator.GetFloat("MoveSpeed") - 0.01f);
            else
                PlayerAnimator.SetBool("MoveForward", false);
        }
        PlayerAnimator.SetBool("MoveBack", Input.GetAxis("Vertical") < 0);
        PlayerAnimator.SetBool("MoveRight", Input.GetAxis("Horizontal") > 0);
        PlayerAnimator.SetBool("MoveLeft", Input.GetAxis("Horizontal") < 0);
        if (PlayerAnimator.GetFloat("MoveSpeed") > 0.9f && StaminaDrain)
        {
            StaminaDrain = false;
            StartCoroutine(ReduceStamina());
        }
        else if (PlayerAnimator.GetFloat("MoveSpeed") < 0.1f && StaminaIncrease && playerInventory.InventoryItems[2] < 15 && Input.GetAxis("Sprint") == 0)
        {
            StaminaIncrease = false;
            StartCoroutine(IncreaseStamina());
        }
    }
    IEnumerator ReduceStamina()
    {
        yield return new WaitForSeconds(2);
        playerInventory.InventoryItems[2] -= 1;
        StaminaDrain = true;
    }
    IEnumerator IncreaseStamina()
    {
        yield return new WaitForSeconds(1);
        playerInventory.InventoryItems[2] += 1;
        StaminaIncrease = true;
    }
    void Aim()
    {
        PlayerAnimator.SetBool("Aim",Input.GetAxis("Fire2")!=0);
        if((Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0) && Mathf.RoundToInt(PlayerAnimator.GetFloat("Weapon"))!=0)
        {
            if(PlayerAnimator.GetFloat("AimAndShoot") < 1)
            {
                PlayerAnimator.SetFloat("AimAndShoot", PlayerAnimator.GetFloat("AimAndShoot") + 0.1f);
            }
            else
            {
                if (Input.GetAxis("Vertical") > 0)
                    PlayerAnimator.SetFloat("AimMove", Mathf.Lerp(PlayerAnimator.GetFloat("AimMove"),0,100*Time.deltaTime));
                else if(Input.GetAxis("Vertical") < 0)
                    PlayerAnimator.SetFloat("AimMove", Mathf.Lerp(PlayerAnimator.GetFloat("AimMove"), 1, Time.time));
                else if(Input.GetAxis("Horizontal")>0)
                    PlayerAnimator.SetFloat("AimMove", Mathf.Lerp(PlayerAnimator.GetFloat("AimMove"), 2, Time.time));
                else if (Input.GetAxis("Horizontal") < 0)
                    PlayerAnimator.SetFloat("AimMove", Mathf.Lerp(PlayerAnimator.GetFloat("AimMove"), 3, Time.time));
            }
        }
        else if(Input.GetAxis("Horizontal") ==0 &&  Input.GetAxis("Vertical") == 0 && Mathf.RoundToInt(PlayerAnimator.GetFloat("Weapon")) != 0)
        {
            if (PlayerAnimator.GetFloat("AimAndShoot") > 0)
            {
                PlayerAnimator.SetFloat("AimAndShoot", 0);//PlayerAnimator.GetFloat("AimAndShoot") - 0.1f);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && PlayerAnimator.GetBool("Aim")==true)
            PlayerAnimator.SetTrigger("Dive");
        if(playerWeapons.CurrentWeapon!=null && Time.timeScale==1f)
        {
            if (PlayerAnimator.GetBool("Aim") == true)
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    if (playerWeapons.CurrentWeapon.GetComponent<WeaponCamera>().Status == false)
                    {
                        playerWeapons.CurrentWeapon.GetComponent<WeaponCamera>().GunCameraOn(true);
                        playerWeapons.Reticle.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        playerWeapons.CurrentWeapon.GetComponent<WeaponCamera>().GunCameraOn(false);
                        playerWeapons.Reticle.transform.localPosition = playerWeapons.ReticleStartPos;
                    }
                }
            }
            else
            {
                playerWeapons.CurrentWeapon.GetComponent<WeaponCamera>().GunCameraOn(false);
                playerWeapons.Reticle.transform.localPosition = playerWeapons.ReticleStartPos;
            }
        }
    }
    void DeadEye()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            if (Time.timeScale == 1 && FocusMode==false)
            {
                FocusMode = true;
                Time.timeScale = 0.2f;
                playerAim.PPP.GetSetting<ColorGrading>().saturation.value = -100;
            }
            else if (Time.timeScale == 0.2f)
            {
                FocusMode = false;
                Time.timeScale = 1;
                playerAim.PPP.GetSetting<ColorGrading>().saturation.value = 0;
            }
        }
        if(FocusMode==true && Time.timeScale==0.2f)
        {
            if(playerInventory.InventoryItems[22] > 0)
            {
                if(FocusDecrease==false)
                {
                    FocusDecrease = true;
                    StartCoroutine(ReduceFocus());
                }
            }
            else
            {
                FocusMode = false;
                Time.timeScale = 1;
                playerAim.PPP.GetSetting<ColorGrading>().saturation.value = 0;
            }
        }
        else if(FocusMode==false)
        {
            if (playerInventory.InventoryItems[22] < 15)
            {

                if (FocusIncrease == false)
                {
                    FocusIncrease = true;
                    StartCoroutine(IncreaseFocus());
                }
            }
        }
        
    }
    IEnumerator ReduceFocus()
    {
        yield return new WaitForSeconds(0.1f);
        playerInventory.InventoryItems[22]-=1;
        FocusDecrease = false;
    }
    IEnumerator IncreaseFocus()
    {
        yield return new WaitForSeconds(1);
        playerInventory.InventoryItems[22] += 1;
        FocusIncrease = false;
    }
    void Cover()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerAnimator.GetBool("Aim") == false)
        {
            if (!PlayerAnimator.GetBool("Cover"))
            {
                Debug.DrawRay(transform.position + transform.up, transform.forward, Color.red, 1f);
                if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, 1f))
                {
                    float RotateAngle = Vector3.Angle(new Vector3(0, 0, 1), -hit.normal);
                    Debug.DrawRay(hit.point, -hit.normal, Color.yellow, 100f);
                    float CurrentAngle = transform.localEulerAngles.y <= 180 ? transform.localEulerAngles.y : transform.localEulerAngles.y - 360;
                    RotateAngle = Mathf.Abs(CurrentAngle - RotateAngle) < Mathf.Abs(CurrentAngle + RotateAngle) ? RotateAngle : -RotateAngle;
                    PlayerAnimator.SetBool("Cover", true);
                    CurrentCover = hit.transform.gameObject;
                    StartCoroutine(ChangePlayerRotation(transform.rotation, Quaternion.Euler(transform.localRotation.x, RotateAngle, transform.localRotation.z), 2f));
                    if (ColliderStandorCover)
                        CoverCollider();
                }
            }
            else
            {
                PlayerAnimator.SetBool("Cover", false);
                if (!ColliderStandorCover)
                    StandCollider();
            }
        }
        if (PlayerAnimator.GetBool("Cover"))
        {
            
            float Horizontal = Input.GetAxis("Horizontal");
            
            Debug.DrawRay(transform.position + transform.right+ 0.5f * transform.up, transform.forward, Color.red, 1f);
            Debug.DrawRay(transform.position - transform.right + 0.5f * transform.up, transform.forward, Color.red, 1f);

            if (Horizontal > 0 && !PlayerAnimator.GetBool("Aim"))
            {
                if (PlayerAnimator.GetFloat("CoverDirection") > 0.01)
                    PlayerAnimator.SetFloat("CoverDirection", PlayerAnimator.GetFloat("CoverDirection") - Time.deltaTime);
                if (Physics.Raycast(transform.position + transform.right + 0.5f * transform.up, transform.forward, out hit, 1f))
                {
                    if (hit.transform.gameObject == CurrentCover)
                        PlayerAnimator.SetFloat("CoverMove", Input.GetAxis("Horizontal"));
                    else if (PlayerAnimator.GetFloat("CoverMove") > 0.01f)
                    {
                        PlayerAnimator.SetFloat("CoverMove", 0);
                    }
                }
                else 
                {
                    if (PlayerAnimator.GetFloat("CoverMove") > 0.01f)
                    {
                        PlayerAnimator.SetFloat("CoverMove", PlayerAnimator.GetFloat("CoverMove") - 0.01f);
                    }
                }
                CoverDir = PlayerAnimator.GetFloat("CoverDirection");

            }
            else if(Horizontal < 0 && !PlayerAnimator.GetBool("Aim"))
            {
                if (PlayerAnimator.GetFloat("CoverDirection") < 0.99)
                    PlayerAnimator.SetFloat("CoverDirection", PlayerAnimator.GetFloat("CoverDirection") + Time.deltaTime);
                if (Physics.Raycast(transform.position - transform.right + 0.5f * transform.up, transform.forward, out hit, 1f))
                {
                    if (hit.transform.gameObject == CurrentCover)
                        PlayerAnimator.SetFloat("CoverMove", Input.GetAxis("Horizontal"));
                    else if (PlayerAnimator.GetFloat("CoverMove") < -0.01f)
                    {
                        PlayerAnimator.SetFloat("CoverMove", 0);
                    }
                }
                else 
                {
                    if (PlayerAnimator.GetFloat("CoverMove") < -0.01f)
                    {
                        PlayerAnimator.SetFloat("CoverMove", PlayerAnimator.GetFloat("CoverMove") + 0.01f);
                    }
                }
                CoverDir = PlayerAnimator.GetFloat("CoverDirection");

            }
            else if(Mathf.RoundToInt(Horizontal)==0)
            {
                if(PlayerAnimator.GetBool("Aim"))
                {

                    Debug.DrawRay(transform.position + 1.5f * transform.up, transform.forward, Color.blue);
                    if (!Physics.Raycast(transform.position+1.5f*transform.up, transform.forward, out hit, 5f))
                    {
                        if (!ColliderStandorCover)
                            StandCollider();
                        PlayerAnimator.SetFloat("CoverDirection", Mathf.Lerp(PlayerAnimator.GetFloat("CoverDirection"), 2, 5 * Time.deltaTime));
                        if (ChangePosition == false)
                        {
                            CoverDir = PlayerAnimator.GetFloat("CoverDirection");
                            ChangePosition = true;
                            PreviousPosition = transform.position;
                            PreviousRotation = transform.localRotation;
                        }
                        PlayerAnimator.SetFloat("CoverAim", 0);
                    } 
                    else if (!Physics.Raycast(transform.position + transform.right + transform.up, transform.forward, out hit, 5f))
                    {
                        if (!ColliderStandorCover)
                            StandCollider();
                        PlayerAnimator.SetFloat("CoverDirection", Mathf.Lerp(PlayerAnimator.GetFloat("CoverDirection"), 2, 5*Time.deltaTime));
                        if (ChangePosition == false)
                        {
                            ChangePosition = true;
                            PreviousPosition = transform.position;
                            PreviousRotation = transform.rotation;
                            StartCoroutine(ChangePlayerPosition(transform.position, transform.position + transform.right, 1f));
                        }
                        PlayerAnimator.SetFloat("CoverAim", 0);
                    }
                    else if (!Physics.Raycast(transform.position - transform.right+transform.up, transform.forward, out hit, 2f))
                    {
                        if (!ColliderStandorCover)
                            StandCollider();
                        PlayerAnimator.SetFloat("CoverDirection", Mathf.Lerp(PlayerAnimator.GetFloat("CoverDirection"), 2, 5 * Time.deltaTime));
                        if (ChangePosition == false)
                        {
                            ChangePosition = true;
                            PreviousPosition = transform.position;
                            PreviousRotation = transform.rotation;
                            StartCoroutine(ChangePlayerPosition(transform.position, transform.position - transform.right, 1f));                            
                        }
                        PlayerAnimator.SetFloat("CoverAim", 0);
                    }
                }
                else
                {
                    if(ChangePosition==true)
                    {
                        ChangePosition = false;
                        if (PreviousPosition != null)
                        {
                            StartCoroutine(ChangePlayerPosition(transform.position, PreviousPosition, 1f));
                            transform.position = PreviousPosition;
                            PreviousPosition = Vector3.zero;
                            if (ColliderStandorCover)
                                CoverCollider();
                        }
                        StartCoroutine(ChangePlayerRotation(transform.rotation,PreviousRotation, 2f));
                        
                    }
                    PlayerAnimator.SetFloat("CoverDirection", Mathf.Lerp(PlayerAnimator.GetFloat("CoverDirection"), CoverDir ,5 * Time.deltaTime));

                }
                if (PlayerAnimator.GetFloat("CoverMove")<-0.01f)
                {
                    PlayerAnimator.SetFloat("CoverMove", PlayerAnimator.GetFloat("CoverMove") + 0.01f);
                }
                else if (PlayerAnimator.GetFloat("CoverMove") > 0.01f)
                {
                    PlayerAnimator.SetFloat("CoverMove", PlayerAnimator.GetFloat("CoverMove") - 0.01f);
                }
            }
        }    
    }
    IEnumerator ChangePlayerPosition(Vector3 start, Vector3 end, float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            transform.position = Vector3.Lerp(start, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
    IEnumerator ChangePlayerRotation(Quaternion start, Quaternion end, float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
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
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            if (playerInventory.InventoryItems[0] <= 0 && PlayerAnimator.enabled)
            {
                PlayerAnimator.enabled = false;
                playerAim.enabled = false;
                playerInventory.enabled = false;
                playerWeapons.enabled = false;
                Ragdoll(true);
                Destroy(PlayerController);
                Destroy(PlayerCollider);
            }
            else
            {
                playerInventory.InventoryItems[0]--;
            }
        }
        

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ZombieHands")
        {

            if (playerInventory.InventoryItems[0] <= 0 && PlayerAnimator.enabled)
            {
                PlayerAnimator.enabled = false;
                playerAim.enabled = false;
                playerInventory.enabled = false;
                playerWeapons.enabled = false;
                Ragdoll(true);
                Destroy(PlayerController);
                Destroy(PlayerCollider);
            }
            else
            {
                playerInventory.InventoryItems[0]--;
            }
        }
        if (other.gameObject.tag == "ZombieHead")
        {
            if (playerInventory.InventoryItems[0] <= 0 && PlayerAnimator.enabled)
            {
                PlayerAnimator.enabled = false;
                playerAim.enabled = false;
                playerInventory.enabled = false;
                playerWeapons.enabled = false;
                Ragdoll(true);
                Destroy(PlayerController);
                Destroy(PlayerCollider);
            }
            else
            {
                playerInventory.InventoryItems[0] -= 5;
            }
        }
    }
    void CheckIfFalling()
    {
        if(PlayerController.velocity.y<-20f)
        {
            PlayerAnimator.enabled = false;
            playerAim.enabled = false;
            playerInventory.enabled = false;
            playerWeapons.enabled = false;
            Ragdoll(true);
            Destroy(PlayerController);
            Destroy(PlayerCollider);
        }
    }
    public void Death()
    {
        PlayerAnimator.enabled = false;
        playerAim.enabled = false;
        playerInventory.enabled = false;
        playerWeapons.enabled = false;
        Ragdoll(true);
        Destroy(PlayerController);
        Destroy(PlayerCollider);
    }
    private void StandCollider()
    {
        PlayerController.height = 1.7f;
        PlayerCollider.height= 1.7f;
        PlayerController.center = new Vector3(0, 0.89f, 0);
        PlayerCollider.center = new Vector3(0, 0.89f, 0);
        ColliderStandorCover = true;
    }
    private void CoverCollider()
    {
        PlayerController.height = 1.13f;
        PlayerCollider.height = 1.13f;
        PlayerController.center = new Vector3(0, 0.61f, 0);
        PlayerCollider.center = new Vector3(0, 0.61f, 0);
        ColliderStandorCover = false;
    }
    public void TurnAnimationOff(string AnimationName)
    {
        PlayerAnimator.SetBool(AnimationName, false);
    }
    
    public void LoadGame(PlayerData data)
    {
        transform.position = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
    }
    public void PlayAudioOnce(AudioClip Clip)
    {
        PlayerAudioSource.PlayOneShot(Clip);
    }
    private void OnDisable()
    {
        if (FocusMode == true)
        {
            FocusMode = false;
            Time.timeScale = 1;
            playerAim.PPP.GetSetting<ColorGrading>().saturation.value = 0;
        }
    }
}
