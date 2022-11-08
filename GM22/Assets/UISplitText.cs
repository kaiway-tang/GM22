using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplitText : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Color colChange;
    [SerializeField] float spd;
    void OnDisable()
    {
        rend.color += colChange*99;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rend.color -= colChange;
        trfm.position += trfm.right * spd;
    }
}
