using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDetection : MonoBehaviour
{
    private Enemy enemyScript;
    void Start()
    {
        enemyScript = transform.parent.gameObject.GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Bullet>() && other.gameObject.tag!="EnemyBullet"  && enemyScript.enemyBehaviour!=Enemy.EnemyBehaviour.FindAndHuntPlayer)
        {
            enemyScript.enemyBehaviour = Enemy.EnemyBehaviour.FindAndHuntPlayer;
            gameObject.SetActive(false);
        }
        else
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>());
        }
    }
}
