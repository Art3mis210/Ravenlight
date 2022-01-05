using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    private Animator PlayerAnimator;
    private RaycastHit hit;
    private bool ClimbEnable;
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
       /*
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.DrawRay(transform.position+2 * transform.up, transform.forward, Color.red, 2f);
            Debug.DrawRay(transform.position+3*transform.up, transform.forward, Color.red, 2f);
            if (PlayerAnimator.GetBool("Hang") != true && Physics.Raycast(transform.position+2 * transform.up, transform.forward,out hit,2f))
            {
                
                float Height=2*(hit.transform.GetComponent<Collider>().bounds.extents.y);
                if (!Physics.Raycast(transform.position+ 3 * transform.up, transform.forward,out hit,2f) && Height<=5)
                {
                     PlayerAnimator.SetBool("Hang", true);
                     StartCoroutine(SetHangingPosition(Height,transform.position.y,1f));
                    
                     
                }
            }
            else if(ClimbEnable==true)
            {
                PlayerAnimator.SetBool("Hang", false);
                gameObject.GetComponent<CharacterController>().enabled = false;
                StartCoroutine(ClimbPosition(transform.position.y,1f));
            }
        } */
    }
    IEnumerator SetHangingPosition(float Height,float CurrentY, float Duration)
    {
            float t = 0f;
            while (t < Duration)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,CurrentY+0.7f-2*(Height/10),transform.position.z), t / Duration);
                yield return null;
                t += Time.deltaTime;
            }
            ClimbEnable = true;
    }
    IEnumerator ClimbPosition(float CurrentHeight,float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, CurrentHeight+1f,transform.position.z), t / Duration);
            yield return null;
            t += Time.deltaTime;
            
        }
        ClimbEnable = false;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }
}
