using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public bool DetectPlayer;
    void Start()
    {
        DetectPlayer = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            DetectPlayer = true;
        }
    }
    public void ChangeLocation(Vector3 Location)
    {
        DetectPlayer = false;
        transform.position = Location;
    }
}
