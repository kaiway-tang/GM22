using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MobileEntity
{
    private GM22 inputs;
    private InputAction attackInputAction;
    private InputAction moveInputAction;

    private InputAction sprintInputAction;
    
    Animator anim;
    [SerializeField] float speed = 5f;

    [Tooltip("The mesh object that is moving in the animations. We use this to set the position of the transform.")]
    [SerializeField] GameObject animObject;
    [SerializeField] GameObject cam;
    [SerializeField] List<GameObject> slashVFX;
    [SerializeField] GameObject strikeVFX;
    [SerializeField] Material chargeGlow;
    [SerializeField] List<Color> glowColors;
    [SerializeField] GameObject swordFX;
    VisualEffect chargeFX;

    int charge = 0;
    int maxStage = 3;
    float holdTime = 0;  // Mouse held time in seconds
    [Tooltip("Hold time needed to initiate charge attack.")] [SerializeField] float holdTimeForCharge = 0.5f;
    [Tooltip("Time required to charge up 1 state")] [SerializeField] float timePerChargePhase = 1f;
    bool swinging = false;
    int combo = 0;
    bool sprinting = false;
    [SerializeField] float maxTimeBetweenCombo = 0.6f;
    [SerializeField] float sprintModifier = 2f;
    float comboTimer = 0;


    // Player stats
    [SerializeField] float damageMult = 1; 
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float moveSpeed = 1;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
        sprintInputAction = inputs.Player.Sprint;
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

            float totSpeed = speed * moveSpeed;  // Base speed (speed) times speed multiplier (moveSpeed)

            if (sprinting) totSpeed *= sprintModifier;

            // Always face direction of movement
            if (!notMoving) transform.forward = Vector3.Lerp(transform.forward, direction, 0.9f);

            // Assume no vertical movement/gravity as of rn (so y velocity ALWAYS 0)
            if (!notMoving)
                rb.velocity = new Vector3(direction.x * totSpeed, rb.velocity.y, direction.z * totSpeed);
            else
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
        } else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        rb.angularVelocity = Vector3.zero;

        // Control animations
        anim.SetBool("Moving", rb.velocity.magnitude - rb.velocity.y > 0.05f);
        anim.SetFloat("atkSpeed", attackSpeed);
        if (sprinting)
            anim.SetFloat("moveSpeed", moveSpeed * sprintModifier);
        else
            anim.SetFloat("moveSpeed", moveSpeed);

        // Control combo state
        AnimatorStateInfo curState = anim.GetCurrentAnimatorStateInfo(0);
        // Only count down when player is not swinging
        if (comboTimer > 0 && (curState.IsName("Idle") || curState.IsName("Run")))
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                combo = 0;
                comboTimer = 0;
                anim.SetBool("Combo", false);
            }
        }

        if (attackInputAction.triggered)
        {
            anim.SetBool("Combo", true);
            if (comboTimer > 0)
            {
                combo++;
                if (combo > 2) combo = 0;
                comboTimer = 0;
            }
            anim.SetInteger("ComboCount", combo);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            // SpawnStrike();  // Purely testing
        }

        sprinting = sprintInputAction.IsPressed();

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
                    Debug.Log("levelUp");
                    chargeFX.SendEvent("levelUp");
                    StartCoroutine(Flash());
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

        HandleAnims();

        chargeFX.SetInt("chargeLevel", charge);
    }

    void HandleAnims()
    {
        // Check the current animation to see if we apply speed changes
        AnimatorStateInfo curState = anim.GetCurrentAnimatorStateInfo(0);
        if (curState.IsName("Idle") || curState.IsName("Charge") || curState.IsName("Charge Swing"))
        {
            // Don't modify speed at all
        }
        else if (curState.IsName("Run"))  
        {
            // Modify anim speed based on player speed
        } else
        {
            // Modify anim based on attack speed 
        }
    }

    IEnumerator Flash()
    {
        float str = 1f;  // tested magic numbers
        Color color = glowColors[charge];
        Debug.Log(chargeGlow.GetFloat("_Strength"));
        chargeGlow.SetFloat("_Strength", str);
        chargeGlow.SetColor("_Color", color);
        while (str < 2.6f)
        {
            str = Mathf.Lerp(str, 3.8f, 0.02f);
            chargeGlow.SetFloat("_Strength", str);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ActivateSlash(int index)
    {
        slashVFX[index].SetActive(true);
        slashVFX[index].GetComponent<PlayerHitbox>().damage = (int)Mathf.Round(1 * damageMult);
    }

    public void DeactivateSlash(int index)
    {
        slashVFX[index].SetActive(false);
    }

    public void ResetCombo()
    {
        comboTimer = maxTimeBetweenCombo;
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
        if (GameManager.gamePaused) { return; }
        transform.position += new Vector3(animObject.transform.localPosition.x, 0, animObject.transform.localPosition.z);
    }

    public void SpawnStrike()
    {
        Vector3 pos = transform.position + Vector3.up * 0f;
        if (charge < 2)
        {
            Instantiate(strikeVFX, pos, transform.rotation);
        } else if (charge < 3)
        {
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * 15));
            Instantiate(strikeVFX, pos, transform.rotation);
            GameObject l1 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, pos, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * -15));
        } else
        {
            Instantiate(strikeVFX, pos, transform.rotation);
            GameObject l1 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, pos, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            GameObject l2 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r2 = Instantiate(strikeVFX, pos, transform.rotation);
            l2.transform.Rotate(Vector3.up, 30);
            r2.transform.Rotate(Vector3.up, -30);
        }
        charge = 0;
        chargeGlow.SetColor("_Color", glowColors[0]);
    }

}
