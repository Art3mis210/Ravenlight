using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    public LadderManager LadderState;
    public bool Down;
    private Animator PlayerAnimator;
    private GameObject Player;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>())
        {
            PlayerAnimator = other.gameObject.GetComponent<Animator>();
            Player = other.gameObject;
            if (!LadderState.OnLadder && Input.GetKey(KeyCode.E))
            {
                LadderState.OnLadder = true;
                if (Down)
                {
                    PlayerAnimator.SetFloat("LadderDir", -1f);
                    PlayerAnimator.SetBool("Ladder", true);
                    StartCoroutine(MovePlayer(Player.transform.position, transform.position, 1f));
                    StartCoroutine(RotatePlayer(Player.transform.rotation, transform.rotation, 1f));
                }
                else
                {
                    PlayerAnimator.SetFloat("LadderDir", 1f);
                    PlayerAnimator.SetBool("Ladder", true);
                    StartCoroutine(MovePlayer(Player.transform.position, transform.position-0.05f*transform.forward, 1f));
                    StartCoroutine(RotatePlayer(Player.transform.rotation, transform.rotation, 1f));
                }
            }
            else if(LadderState.OnLadder)
            {
                
                if (Down && PlayerAnimator.GetBool("MoveBack"))
                {
                    PlayerAnimator = other.gameObject.GetComponent<Animator>();
                    PlayerAnimator.SetBool("Ladder", false);
                    LadderState.OnLadder = false;
                }
                else if(!Down && PlayerAnimator.GetBool("MoveForward"))
                {
                    PlayerAnimator = other.gameObject.GetComponent<Animator>();
                    PlayerAnimator.SetBool("Ladder", false);
                    LadderState.OnLadder = false;
                    StartCoroutine(MovePlayer(Player.transform.position, Player.transform.position + Player.transform.forward, 1f));
                }
            }
        }
    }
    IEnumerator RotatePlayer(Quaternion start, Quaternion end, float Duration)
    {
        float t = 0f;
        end = new Quaternion(0, end.y, 0, end.w);
        while (t < Duration)
        {
            Player.transform.localRotation = Quaternion.Slerp(start, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
    IEnumerator MovePlayer(Vector3 start, Vector3 end, float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            Player.transform.position = Vector3.Lerp(start, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }


}
