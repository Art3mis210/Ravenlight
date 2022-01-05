using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyRandomEvent : MonoBehaviour
{
    public RandomMission randomMission;
    private Animator EnemyAnimator;
    bool CheckEnemy;
    private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        CheckEnemy = false;
    }
    void Update()
    {
        if(CheckEnemy==false && EnemyAnimator.enabled==false)
        {
            CheckEnemy = true;
            randomMission.EnemiesKilled++;
            this.enabled = false;
        }
    }

}
