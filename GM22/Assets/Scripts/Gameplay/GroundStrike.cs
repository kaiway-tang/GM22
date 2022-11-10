using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundStrike : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [Tooltip("Height this strike will check to find a ground and warp to. If it finds nothing, it will move to y=0.")]
    [SerializeField] float detectionDistance = 2f;
    [SerializeField] LayerMask layers;

    [SerializeField] float slowRate = 0.1f;

    Rigidbody rb;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set initial travel speed
        rb.velocity = transform.forward * speed;
        StartCoroutine(Decelerate());
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * .7f, transform.TransformDirection(Vector3.down), out hit, detectionDistance, layers))
        {
            transform.parent.position = new Vector3(transform.parent.position.x, hit.point.y, transform.parent.position.z);
        } else
        {
            transform.parent.position = new Vector3(transform.parent.position.x, 0, transform.parent.position.z);
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
        Destroy(transform.parent.gameObject);
    }
}
