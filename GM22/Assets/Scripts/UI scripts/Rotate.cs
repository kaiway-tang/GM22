using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * 2);
    }
}
