using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChanger : MonoBehaviour
{
    public Transform ChangeTarget()
    {
        return transform.parent.GetComponent<Targets>().ChangeTarget(transform);
    }
}
