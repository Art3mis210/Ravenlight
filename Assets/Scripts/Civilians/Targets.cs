using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
    public List<Transform> TargetList;
    private void Awake()
    {
        TargetList = new List<Transform>();
        foreach(Transform child in transform)
        {
            TargetList.Add(child.transform);
        }    
    }
    public Transform ChangeTarget(Transform PreviousTarget)
    {
        Transform NewTarget;
        do
        {
            NewTarget = TargetList[Random.Range(0, TargetList.Count-1)];
        } while (NewTarget == PreviousTarget);
        return NewTarget;
    }
    public Transform AssignTarget()
    {
        Transform NewTarget;
        NewTarget = TargetList[Random.Range(0, TargetList.Count-1)];
        return NewTarget;
    }
}
