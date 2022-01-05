using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerWeapons>().Reload();
    }
}
