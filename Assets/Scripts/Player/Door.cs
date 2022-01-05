using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Right;
    private bool Rotate;
    private void Start()
    {
        
    }
     void Update()
     {
         Debug.DrawRay(transform.position, transform.forward, Color.red, 2f);
     }
     private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.tag == "Player")
         {
             if (Rotate == false)
             {
                 if (Right == true)
                     StartCoroutine(RotateDoor(1f, transform.localRotation, Quaternion.Euler(0, 90, 0)));
                 else
                     StartCoroutine(RotateDoor(1f, transform.localRotation, Quaternion.Euler(0, -90, 0)));
             }
         }
     }
     IEnumerator RotateDoor(float Duration, Quaternion start,Quaternion end)
     {
         float t = 0f;
         while (t < Duration)
         {
             transform.localRotation = Quaternion.Slerp(start, end, t / Duration);
             yield return null;
             t += Time.deltaTime;
         }
         transform.localRotation = end;
         Rotate = false;
         Destroy(this);
     }
}
