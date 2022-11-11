using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDest : MonoBehaviour
{
    //destroys this gameobject in "time" seconds

    [SerializeField] float time;
    void Start()
    {
        Destroy(gameObject, time);
    }
}
