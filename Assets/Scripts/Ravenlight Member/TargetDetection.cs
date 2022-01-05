using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    private RavenlightMember ravenlightMember;
    private void Start()
    {
        ravenlightMember = transform.parent.GetComponent<RavenlightMember>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Zombie")
        {
            if (other.gameObject.GetComponent<Animator>())
            {
                if (other.gameObject.GetComponent<Animator>().enabled)
                    ravenlightMember.Targets.Add(other.gameObject);
                else
                {
                    Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), transform.GetComponent<Collider>());
                }
            }
        }
        else
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), transform.GetComponent<Collider>());
        }
    
        
    }
}
