using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundStrike : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [Tooltip("Height this strike will check to find a ground and warp to. If it finds nothing, it will move to y=0.")]
    [SerializeField] float detectionDistance = 2f;

    [SerializeField] float slowRate = 0.1f;

    Rigidbody rb;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set initial travel speed
        rb.velocity = transform.forward * 10f;
        StartCoroutine(Decelerate());
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out hit, detectionDistance))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        } else
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    IEnumerator Decelerate()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowRate;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
