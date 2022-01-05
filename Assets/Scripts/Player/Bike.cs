using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bike : MonoBehaviour
{
    private BikeControl DirtBike;
    public GameObject PlayerOnBike;
    public PlayerThirdPersonCamera PlayerCamera;

    private GameObject Player;
    private Player PlayerScript;
    private PlayerAim PlayerSecondScript;
    private GameObject PlayerMesh;
    private CharacterController PlayerController;
    private CapsuleCollider PlayerCollider;
    private GameObject PlayerRoot;
    public GameObject BikeSound;

    private BoxCollider BikeCollider;
    private Inventory PlayerInventory;

    private float TimeOnBike;
    private bool BikeFirstTime;
    public bool OnBike;

    public GameObject BikeSprite;
    public GameObject BikeIndication;
    void Start()
    {
        OnBike = false;
        DirtBike = GetComponent<BikeControl>();
        BikeFirstTime = true;
        TimeOnBike = -1f;
        BikeSound.SetActive(false);
        BikeCollider = GetComponent<BoxCollider>();
        BikeIndication.SetActive(false);
    }
    void Update()
    {
        if(OnBike==true)
        {
            if(Player.transform.localRotation!= Quaternion.Euler(0f, 0f, 0f))
                Player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            if (Player.transform.localPosition != new Vector3(0, 0, 0))
                Player.transform.localPosition = new Vector3(0, 0, 0);
            TimeOnBike += Time.deltaTime;
            if (Input.GetKey(KeyCode.F) && TimeOnBike>1f)
                GetOffBike();
        }
        else
        {
            TimeOnBike -= Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Player>() && other.gameObject.GetComponent<Player>())
        {
            BikeIndication.SetActive(true);
            if(Input.GetKey(KeyCode.F) && OnBike==false && TimeOnBike<0)
            {
                BikeIndication.SetActive(false);
                TimeOnBike = 0;
                OnBike = true;
                if(BikeFirstTime==true)
                {
                    Player = other.gameObject;
                    PlayerScript = Player.GetComponent<Player>();
                    PlayerSecondScript = Player.GetComponent<PlayerAim>();
                    PlayerController= Player.GetComponent<CharacterController>();
                    PlayerCollider = Player.GetComponent<CapsuleCollider>();
                    PlayerMesh = Player.transform.GetChild(0).gameObject;
                    PlayerRoot = Player.transform.GetChild(1).gameObject;
                    PlayerInventory = Player.GetComponent<Inventory>();
                }
                PlayerSetActive(false);
                PlayerOnBike.SetActive(true);
                PlayerCamera.ChangeTarget(transform);
                DirtBike.activeControl = true;
                UpdatePlayerOnBike();
                Player.transform.parent = DirtBike.transform;
                Player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                Player.transform.localPosition = new Vector3(0,0,0);
                BikeSound.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            BikeIndication.SetActive(false);
        }
    }
    private void UpdatePlayerOnBike()
    {
        GameObject PlayerOnBikeMesh=PlayerOnBike.transform.GetChild(0).gameObject;
        for (int i=0;i<PlayerMesh.transform.childCount;i++)
        {
            PlayerOnBikeMesh.transform.GetChild(i).gameObject.SetActive(PlayerMesh.transform.GetChild(i).gameObject.activeSelf);
        }
    }
    private void GetOffBike()
    {
        Player.transform.parent = null;
        DirtBike.activeControl = false;
        OnBike = false;
        PlayerOnBike.SetActive(false);
        Player.transform.parent = null;
        PlayerCamera.ChangeTarget(Player.transform);
        Player.transform.position = transform.position-transform.right;
        PlayerSetActive(true);
        TimeOnBike = 1f;
        BikeSound.SetActive(false);
    }
    private void PlayerSetActive(bool Status)
    {
        PlayerScript.enabled = Status;
        PlayerSecondScript.enabled = Status;
        PlayerCollider.enabled = Status;
        PlayerController.enabled = Status;
        PlayerMesh.SetActive(Status);
        PlayerRoot.SetActive(Status);
        BikeSprite.SetActive(Status);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(OnBike==true)
        {
            if(other.gameObject.tag=="EnemyBullet")
            {
                if(PlayerInventory.InventoryItems[0]>0)
                    PlayerInventory.InventoryItems[0]--;
                else
                {
                    GetOffBike();
                  // PlayerScript.
                    PlayerScript.Death();
                }

            }
        }
    }
}
