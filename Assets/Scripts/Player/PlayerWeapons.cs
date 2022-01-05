using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerWeapons : MonoBehaviour
{

    public List<GameObject> PrimaryWeapons;
    public List<GameObject> SecondaryWeapons;
    public List<GameObject> Weapons;
    private Animator PlayerAnimator;
    public Transform GunPosIdle;
    public Transform GunPosSprint;
    public Transform GunPosAim;
    public CinemachineVirtualCamera AimCamera;
    public GameObject CurrentWeapon;
    private int CurrentWeaponSlot;
    private PlayerAim playerAim;
    public GameObject Reticle;
    public GameObject WeaponTarget;
    private RaycastHit hit;
    public Text AmmoText;
    private Inventory playerInventory;
    private Player player;
    public Vector3 ReticleStartPos;
    private bool ChangingGun;
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        CurrentWeapon = null;
        CurrentWeaponSlot = 0;
        playerAim = GetComponent<PlayerAim>();
        playerInventory = GetComponent<Inventory>();
        player = GetComponent<Player>();
        ReticleStartPos = Reticle.transform.localPosition;
        ChangingGun = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(CurrentWeaponSlot!=0 && player.enabled==true)
        {
            if (Input.GetMouseButton(0) && PlayerAnimator.GetBool("Aim"))
            {
                float Recoil=CurrentWeapon.GetComponent<Weapons>().Shoot();
                if(Recoil>0)
                    WeaponTarget.transform.position = new Vector3(WeaponTarget.transform.position.x, WeaponTarget.transform.position.y + Recoil, WeaponTarget.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if ((CurrentWeaponSlot == 1 && playerInventory.InventoryItems[4] > 0) || (CurrentWeaponSlot == -1 && playerInventory.InventoryItems[6] > 0))
                {
                    CurrentWeapon.GetComponent<Weapons>().ReloadSound();
                    if (PlayerAnimator.GetBool("Cover"))
                    {
                        if (Mathf.RoundToInt(PlayerAnimator.GetFloat("CoverDirection")) == 0)
                        {
                            PlayerAnimator.SetInteger("ReloadDir", 1);
                            PlayerAnimator.SetBool("Reload", true);
                        }
                        else
                        {
                            PlayerAnimator.SetInteger("ReloadDir", -1);
                            PlayerAnimator.SetBool("Reload", true);
                        }
                    }
                    else
                    {
                        PlayerAnimator.SetInteger("ReloadDir", 0);
                        PlayerAnimator.SetBool("Reload", true);
                    }
                }
            }
            
        }
        if (AmmoText.transform.gameObject.activeInHierarchy && CurrentWeapon!=null)
            if(CurrentWeaponSlot==1)
                AmmoText.text = CurrentWeapon.GetComponent<Weapons>().GetBullets().ToString()+"/"+playerInventory.InventoryItems[4].ToString();
            else 
                AmmoText.text = CurrentWeapon.GetComponent<Weapons>().GetBullets().ToString() + "/" + playerInventory.InventoryItems[6].ToString();
        WeaponAimCamera();
        //GunPosition();
        
    }
    public void ChangeWeapon(int Weapon)
    {
        if (Weapon==0)
        {
            if (CurrentWeaponSlot != 0 && ChangingGun==false)
            {
                ChangingGun = true;
                if (CurrentWeaponSlot > 0)
                    StartCoroutine(ChangeWeaponDecrease(CurrentWeaponSlot, 0));
                else if (CurrentWeaponSlot < 0)
                {
                    StartCoroutine(ChangeWeaponIncrease(CurrentWeaponSlot, 0));
                }
                AmmoText.transform.gameObject.SetActive(false);
            }
            
        }
        if (Weapon==1)
        {
            if (CurrentWeaponSlot != 1 &&  ChangingGun == false)
            {
                if (CurrentWeaponSlot > 1)
                    StartCoroutine(ChangeWeaponDecrease(CurrentWeaponSlot, 1));
                else if (CurrentWeaponSlot < 1)
                {
                    StartCoroutine(ChangeWeaponIncrease(CurrentWeaponSlot, 1));
                }
                AmmoText.transform.gameObject.SetActive(true);
            }
        }
        if (Weapon==2)
        {
            if (CurrentWeaponSlot != -1  && ChangingGun == false)
            {
                if (CurrentWeaponSlot > -1)
                    StartCoroutine(ChangeWeaponDecrease(CurrentWeaponSlot, -1));
                else if (CurrentWeaponSlot < -1)
                {
                    StartCoroutine(ChangeWeaponIncrease(CurrentWeaponSlot, -1));
                }
                AmmoText.transform.gameObject.SetActive(true);
            }
        }
    }
    private void WeaponAimCamera()
    {
        if (PlayerAnimator.GetBool("Aim") && Input.GetMouseButton(1) && CurrentWeaponSlot != 0 && playerAim.WeaponIKAim==true)
        {
            AimCamera.enabled = true;
            AimCamera.Follow = CurrentWeapon.transform;
            AimCamera.LookAt = CurrentWeapon.transform;
            if (!Reticle.activeInHierarchy)
                Reticle.SetActive(true);
           // Debug.DrawRay(CurrentWeapon.transform.position, 10 * CurrentWeapon.transform.forward, Color.red, 0.01f);

        }
        else
        {
            AimCamera.Follow = null;
            AimCamera.LookAt = null;
            AimCamera.enabled = false;
            if (Reticle.activeInHierarchy)
                Reticle.SetActive(false);
        }
    }
    public void Reload()
    {
        if(CurrentWeaponSlot==1)
        {
            if (CurrentWeapon != null)
                playerInventory.InventoryItems[4]=CurrentWeapon.GetComponent<Weapons>().ReloadWeapon(playerInventory.InventoryItems[4]);
           
        }
        else
        {
            if (CurrentWeapon != null)
                playerInventory.InventoryItems[6] = CurrentWeapon.GetComponent<Weapons>().ReloadWeapon(playerInventory.InventoryItems[6]);
        }
        
    }
    private void GunPosition()
    {
        if (CurrentWeaponSlot==1)
        {
            if (PlayerAnimator.GetFloat("MoveSpeed") > 0 && PlayerAnimator.GetFloat("AimAndShoot") == 0)
            {
                CurrentWeapon.transform.localPosition = GunPosSprint.transform.localPosition;
                CurrentWeapon.transform.localRotation = GunPosSprint.transform.localRotation;
            }
            else if (PlayerAnimator.GetFloat("MoveSpeed") == 0 && PlayerAnimator.GetFloat("AimAndShoot") == 0 && !PlayerAnimator.GetBool("MoveForward"))
            {
                CurrentWeapon.transform.localPosition = GunPosIdle.transform.localPosition;
                CurrentWeapon.transform.localRotation = GunPosIdle.transform.localRotation;
            }
            else if (PlayerAnimator.GetFloat("AimAndShoot") > 0 && PlayerAnimator.GetFloat("MoveSpeed") == 0 && !PlayerAnimator.GetBool("MoveForward"))
            {
                CurrentWeapon.transform.localPosition = GunPosAim.transform.localPosition;
                CurrentWeapon.transform.localRotation = GunPosAim.transform.localRotation;
            }
        }
    }
    IEnumerator ChangeWeaponIncrease(float a,float b)
    {
        if(CurrentWeapon!=null)
            CurrentWeapon.SetActive(false);
        while (a<b)
        {
            a += Time.deltaTime;
            PlayerAnimator.SetFloat("Weapon", a);
            yield return null;
        }
        CurrentWeaponSlot = Mathf.RoundToInt(b) < 0 ? -1 : Mathf.RoundToInt(b);
        if (Weapons[CurrentWeaponSlot == -1 ? 2 : CurrentWeaponSlot] != null)
            Weapons[CurrentWeaponSlot == -1 ? 2 : CurrentWeaponSlot].SetActive(true);
        CurrentWeapon = Weapons[CurrentWeaponSlot == -1 ? 2 : CurrentWeaponSlot];
        ChangingGun = false;
    }
    IEnumerator ChangeWeaponDecrease(float a, float b)
    {
    //    float t = 0f;
        if (CurrentWeapon != null)
            CurrentWeapon.SetActive(false);
        while (a > b)
        {
            a -= Time.deltaTime;
            PlayerAnimator.SetFloat("Weapon", a);
            yield return null;
        }
        CurrentWeaponSlot = Mathf.RoundToInt(b) < 0 ? -1: Mathf.RoundToInt(b);
        if (Weapons[CurrentWeaponSlot==-1?2:CurrentWeaponSlot] != null)
            Weapons[CurrentWeaponSlot == -1 ? 2 : CurrentWeaponSlot].SetActive(true);
        CurrentWeapon = Weapons[CurrentWeaponSlot == -1 ? 2 : CurrentWeaponSlot];
        ChangingGun = false;
    }
    public void LoadGame(PlayerData data)
    {
        Weapons[1] = PrimaryWeapons[data.Weapons[1]];
        Weapons[2] = SecondaryWeapons[data.Weapons[2]];
    }


}
