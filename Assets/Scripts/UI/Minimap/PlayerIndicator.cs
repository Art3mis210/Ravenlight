using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public Transform Player;
    public float Height;
    void Update()
    {
        transform.position = new Vector3(Player.position.x, Height, Player.position.z);
        transform.rotation = Quaternion.Euler(90, Player.eulerAngles.y, 0);
    }
}
