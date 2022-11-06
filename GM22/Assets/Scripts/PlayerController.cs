using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    [SerializeField] float speed = 5f;

    [Tooltip("The mesh object that is moving in the animations. We use this to set the position of the transform.")]
    [SerializeField] GameObject animObject;
    [SerializeField] GameObject cam;

    bool swinging = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!swinging)
        {
            // Using isometric to test, will change later
            
            Vector3 camDir = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            Vector3 direction = (Input.GetAxis("Horizontal") * new Vector3(camDir.z, 0, -1 * camDir.x) + Input.GetAxis("Vertical") * camDir).normalized;
            
            bool notMoving = direction.magnitude == 0;

            // Always face direction of movement
            if (!notMoving) transform.forward = Vector3.Lerp(transform.forward, direction, 0.9f);

            // Assume no vertical movement/gravity as of rn (so y velocity ALWAYS 0)
            if (!notMoving)
                rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
            else
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
        } else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // Control movement animation
        anim.SetBool("Moving", rb.velocity.magnitude > 0.05f);

        if (Input.GetMouseButton(0))
        {
            anim.SetBool("Combo", true);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
            
    }

    public void ResetCombo()
    {
        anim.SetBool("Combo", false);
    }

    public void SetSwing()
    {
        swinging = true;
    }

    public void ResetSwing()
    {
        swinging = false;
    }

    public void Readjust()
    {
        transform.position += new Vector3(animObject.transform.localPosition.x, 0, animObject.transform.localPosition.z);
    }
}
