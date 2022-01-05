using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    public GameManager gameManager;
    private Animator CheckAnimator;
    void Start()
    {
        CheckAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (CheckAnimator.enabled == false)
        {
            gameManager.KilledCount++;
            this.enabled = false;
        }
    }
}
