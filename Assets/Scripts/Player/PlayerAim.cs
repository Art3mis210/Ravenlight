using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.PostProcessing;

public class PlayerAim : MonoBehaviour
{
    public MultiAimConstraint LookAim;
    public Transform HeadIK;
    public Transform WeaponIK;
    public Camera PlayerCamera;
    private Animator PlayerAnimator;
    public float Sensitivity;
    private float Turn;
    public Vector3 Offset;
    private Transform Chest;
    public bool WeaponIKAim;
    public PostProcessProfile PPP;

    private void Start()
    {
        Turn = 0;
        PlayerAnimator = GetComponent<Animator>();
        Chest = PlayerAnimator.GetBoneTransform(HumanBodyBones.Chest);
        WeaponIKAim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerAnimator.GetBool("Aim") && !PlayerAnimator.GetBool("Ladder"))
        {
            if (PlayerAnimator.GetBool("MoveForward") && Input.GetAxis("Horizontal") == 0 && !PlayerAnimator.GetBool("Cover"))
            {
                StartCoroutine(BringPlayerToCameraDirection(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.forward), 0.5f));
            }
            else if (Input.GetAxis("Horizontal") != 0 && PlayerAnimator.GetBool("MoveForward") && !PlayerAnimator.GetBool("Cover"))
            {
                if (Time.timeScale != 0)
                {
                    Turn += Input.GetAxis("Horizontal") * Sensitivity;
                    transform.localRotation = Quaternion.Euler(0, Turn, 0);
                }
            }
            if (Vector3.Dot((HeadIK.position - transform.position).normalized, transform.forward) < 0 || PlayerAnimator.GetBool("Cover"))
            {
                if (LookAim.weight > 0)
                    LookAim.weight -= Time.deltaTime;
            }
            else
            {
                if (LookAim.weight < 1)
                    LookAim.weight += Time.deltaTime;
            }
        }
        else
        {
            if (LookAim.weight > 0 && PlayerAnimator.GetBool("Aim") && PlayerAnimator.GetFloat("Weapon") != 0 && PlayerAnimator.GetFloat("AimAndShoot") > 0)
                    LookAim.weight -= Time.deltaTime;
            if (!PlayerAnimator.GetBool("Ladder"))
            {
                Turn += Input.GetAxis("Mouse X") * 4f;
                transform.localRotation = Quaternion.Euler(0, Turn, 0);
            }
        }
    }
    private void LateUpdate()
    {
        if (PlayerAnimator.GetBool("Aim") && Mathf.RoundToInt(PlayerAnimator.GetFloat("Weapon")) != 0)
        {
            if (PlayerAnimator.GetBool("Cover") && Mathf.RoundToInt(PlayerAnimator.GetFloat("CoverDirection")) == 2 || !PlayerAnimator.GetBool("Cover"))
            {
                Chest.LookAt(WeaponIK);
                Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
                WeaponIKAim = true;
                PPP.GetSetting<DepthOfField>().focalLength.value = 75;
                
            }
        }
        else
        {
            PPP.GetSetting<DepthOfField>().focalLength.value = 50;
            WeaponIKAim = false;
        }
    }
    IEnumerator BringPlayerToCameraDirection(Quaternion start, Quaternion end, float Duration)
    {
        float t = 0f;
        end = new Quaternion(0, end.y, 0, end.w);
        while (t < Duration)
        {
            transform.localRotation = Quaternion.Slerp(start, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        Turn = transform.eulerAngles.y;
    }
}
