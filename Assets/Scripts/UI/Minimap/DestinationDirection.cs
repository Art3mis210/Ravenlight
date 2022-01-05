using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationDirection : MonoBehaviour
{
    public GameObject Player;
    public GameObject Target;
    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            if (Vector3.Distance(Player.transform.position, Target.transform.position) > 35 && Time.timeScale!=0)
            {
                Vector3 IconDirection = (Player.transform.position - Target.transform.position).normalized;
                transform.position = Player.transform.position - new Vector3(IconDirection.x * 33, 0, IconDirection.z * 33);
            }
            else
            {
                transform.position = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            }
        }
    }
    public void ChangeTarget(GameObject NewTarget)
    {
        Target = NewTarget;
    }
}
