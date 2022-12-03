using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        HPEntity h = collision.gameObject.GetComponent<HPEntity>();
        if (h)
        {
            h.TakeDmg(h.HP);
        }
    }
}
