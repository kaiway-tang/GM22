using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private GM22 inputs;
    private InputAction attackInputAction;
    private InputAction moveInputAction;
    
    Rigidbody rb;
    Animator anim;
    [SerializeField] float speed = 5f;

    [Tooltip("The mesh object that is moving in the animations. We use this to set the position of the transform.")]
    [SerializeField] GameObject animObject;
    [SerializeField] GameObject cam;
    [SerializeField] List<GameObject> slashVFX;
    [SerializeField] GameObject strikeVFX;
    [SerializeField] GameObject swordFX;
    VisualEffect chargeFX;

    int charge = 0;
    int maxStage = 3;
    float holdTime = 0;  // Mouse held time in seconds
    [Tooltip("Hold time needed to initiate charge attack.")] [SerializeField] float holdTimeForCharge = 0.5f;
    [Tooltip("Time required to charge up 1 state")] [SerializeField] float timePerChargePhase = 1f;
    bool swinging = false;


    // Player stats
    int damage = 1;
    float attackSpeed = 1;
    float maxHealth = 100;
    float curHealth = 100;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        chargeFX = swordFX.GetComponentInChildren<VisualEffect>();
        Cursor.lockState = CursorLockMode.Locked;  // Finally remembered to do this
    }

    private void OnEnable()
    {
        inputs = new GM22();
        attackInputAction = inputs.Player.Attack;
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
            rb.angularVelocity = Vector3.zero;
        }

        // Control movement animation
        anim.SetBool("Moving", rb.velocity.magnitude > 0.05f);

        if (attackInputAction.triggered)
        {
            anim.SetBool("Combo", true);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            // SpawnStrike();  // Purely testing
        }

        // True or false based on if it's pressed the current frame
        if (attackInputAction.IsPressed())
        {
            holdTime += Time.deltaTime;
            if (holdTime > holdTimeForCharge)
            {
                int old = charge;
                anim.SetBool("Charge", true);
                charge = (int) ((holdTime - holdTimeForCharge) / timePerChargePhase);
                if (charge > maxStage)
                {
                    charge = maxStage;
                }
                chargeFX.SetInt("chargeLevel", charge);
                if (old != charge)
                {
                    chargeFX.SendEvent("levelUp");
                }
            }
            // Debug.Log(holdTime);
        } else
        {
            if (holdTime > holdTimeForCharge)
            {
                anim.SetBool("Charge", false);
                holdTime = 0;
            } else if (holdTime != 0)
            {
                holdTime = 0;
            }
        }
        chargeFX.SetInt("chargeLevel", charge);
    }

    public void ActivateSlash(int index)
    {
        slashVFX[index].SetActive(true);
        slashVFX[index].GetComponent<PlayerHitbox>().damage = damage;
    }

    public void DeactivateSlash(int index)
    {
        slashVFX[index].SetActive(false);
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
        if (gamem)
        transform.position += new Vector3(animObject.transform.localPosition.x, 0, animObject.transform.localPosition.z);
    }

    public void SpawnStrike()
    {
        if (charge < 2)
        {
            Instantiate(strikeVFX, transform.position, transform.rotation);
        } else if (charge < 3)
        {
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * 15));
            Instantiate(strikeVFX, transform.position, transform.rotation);
            GameObject l1 = Instantiate(strikeVFX, transform.position, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, transform.position, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * -15));
        } else
        {
            Instantiate(strikeVFX, transform.position, transform.rotation);
            GameObject l1 = Instantiate(strikeVFX, transform.position, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, transform.position, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            GameObject l2 = Instantiate(strikeVFX, transform.position, transform.rotation);
            GameObject r2 = Instantiate(strikeVFX, transform.position, transform.rotation);
            l2.transform.Rotate(Vector3.up, 30);
            r2.transform.Rotate(Vector3.up, -30);
        }
        charge = 0;
    }

}
