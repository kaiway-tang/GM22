using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : Attack
{
    [SerializeField] Transform trfm;
    [SerializeField] float spd;

    private void FixedUpdate()
    {
        trfm.position += trfm.forward * spd;
    }

    private new void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }
}
