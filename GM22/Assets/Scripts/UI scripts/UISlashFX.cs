using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlashFX : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] float spd;
    [SerializeField] Vector3 scale;
    Vector3 move = new Vector3(.9848f, .1736f);

    int tmr;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        tmr++;
        trfm.position += trfm.right * spd;
        if (tmr < 7)
        {
            trfm.localScale += scale;
        }
        else if (tmr < 13)
        {
            trfm.localScale -= scale;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
