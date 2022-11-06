using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private GM22 inputs;
    private InputAction attackInputAction;
    private InputAction lookInputAction;
    private InputAction moveInputAction;
    
    Rigidbody rb;
    Animator anim;
    [SerializeField] float speed = 5f;

    [Tooltip("The mesh object that is moving in the animations. We use this to set the position of the transform.")]
    [SerializeField] GameObject animObject;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject slashVFX;

    bool swinging = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputs = new GM22();
        attackInputAction = inputs.Player.Attack;
        lookInputAction = inputs.Player.Look;
        moveInputAction = inputs.Player.Move;
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!swinging)
        {
            // Using isometric to test, will change later
            
            Vector3 camDir = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            Vector3 direction = (moveInputAction.ReadValue<Vector2>().x * new Vector3(camDir.z, 0, -1 * camDir.x) + moveInputAction.ReadValue<Vector2>().y * camDir).normalized;
            
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

        if (attackInputAction.triggered)
        {
            anim.SetBool("Combo", true);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
            
    }

    public void ActivateSlash()
    {
        slashVFX.SetActive(true);
    }

    public void DeactivateSlash()
    {
        slashVFX.SetActive(false);
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
