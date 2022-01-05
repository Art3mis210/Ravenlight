using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Camera MainCamera;
    // Update is called once per frame
    void Update()
    {
        float YPos = transform.localPosition.y + Input.GetAxis("Mouse Y")/4;
        YPos = Mathf.Clamp(YPos, -0.23f, 5f);
        transform.localPosition=new Vector3(transform.localPosition.x,YPos,transform.localPosition.z);
    }
}
