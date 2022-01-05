using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBack : StateMachineBehaviour
{ 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<Inventory>().InventoryItems[0] -= 5;
        if (animator.transform.GetComponent<Inventory>().InventoryItems[0] <= 0)
            animator.transform.GetComponent<Player>().Death();
    }
}
