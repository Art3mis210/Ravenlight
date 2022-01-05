using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AImm : MonoBehaviour
{
    public GameObject Gun;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Gun.transform.position;
        transform.rotation = Gun.transform.rotation;
    }
}
