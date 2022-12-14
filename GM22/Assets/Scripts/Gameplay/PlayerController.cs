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
    private InputAction jumpInputAction;

    private InputAction sprintInputAction;
    private InputAction camlockInputAction;
    
    Animator anim;
    [SerializeField] float speed = 5f, chargeSpdMultiplier = .25f;
    bool chargeSlowed;

    [Tooltip("The mesh object that is moving in the animations. We use this to set the position of the transform.")]
    [SerializeField] GameObject animObject;
    [SerializeField] GameObject cam;
    [SerializeField] YCamera targetCam;
    [SerializeField] List<GameObject> slashVFX;
    [SerializeField] GameObject strikeVFX;
    [SerializeField] GameObject beamVFX;
    [SerializeField] Material chargeGlow;
    [SerializeField] List<Color> glowColors;
    [SerializeField] GameObject swordFX;
    [SerializeField] VisualEffect accumulateVFX;
    VisualEffect chargeFX;
    Transform closestEnemy;
    Transform zLockTarget;

    int charge = 0;
    int maxStage = 3;
    float holdTime = 0;  // Mouse held time in seconds
    [Tooltip("Hold time needed to initiate charge attack.")] [SerializeField] float holdTimeForCharge = 0.5f;
    [Tooltip("Time required to charge up 1 state")] [SerializeField] float timePerChargePhase = 1f;
    bool swinging = false;
    int combo = 0;
    bool sprinting = false;
    bool zLocked = false;
    bool sliding = false;
    [SerializeField] float slideSpeed = 10f;
    float slideCooldown = 3f;
    float cooldown = 0;
    [SerializeField] float maxTimeBetweenCombo = 0.6f;
    [SerializeField] float sprintModifier = 2f;
    float comboTimer = 0;

    // jumping variables
    bool onGround = false;
    bool jumpDesired = false;

    // Player stats
    [SerializeField] float damageMult = 1; 
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float healthBoost = 1;
    [SerializeField] List<int> crystals;

    [SerializeField] private skillPointManager skillPointManager;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        chargeFX = swordFX.GetComponentInChildren<VisualEffect>();
        Cursor.lockState = CursorLockMode.Locked;  // Finally remembered to do this
        crystals = new List<int>();  // Stores player's held crystals
        crystals.Add(0);  // red
        crystals.Add(0);  // green
        crystals.Add(0);  // blue
        Debug.Log("Hello");
    }

    private void OnEnable()
    {
        inputs = new GM22();
        attackInputAction = inputs.Player.Attack;
        jumpInputAction = inputs.Player.Jump;
        moveInputAction = inputs.Player.Move;
        sprintInputAction = inputs.Player.Sprint;
        camlockInputAction = inputs.Player.CamLock;
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEnemies();  // For camera locking
        
        CalculateBuffs();

        //if (camlockInputAction.triggered)
        //{
        //    zLocked = !zLocked;
        //    if (zLocked)
        //    {
        //        targetCam.ZTarget(closestEnemy.gameObject);
        //        zLockTarget = closestEnemy;
        //    }
        //}

        //if (closestEnemy == null)
        //    zLocked = false;

        //if (!zLocked)
        //    targetCam.Normal();
        //anim.SetBool("zLocked", zLocked);

        // Bulk of gameplay code below

        if (true)  // Previously had swing check, no longer necessary
        {
            
            Vector3 camDir = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
            Vector3 direction = (moveInputAction.ReadValue<Vector2>().x * new Vector3(camDir.z, 0, -1 * camDir.x) + moveInputAction.ReadValue<Vector2>().y * camDir).normalized;
            
            bool notMoving = direction.magnitude == 0;

            float totSpeed = speed * moveSpeed * chargeSpdMultiplier;  // Base speed (speed) times speed multiplier (moveSpeed)

            if (sprinting) totSpeed *= sprintModifier;

            if (!sliding)
            {
                // Always face direction of movement, unless you're locked
                if (zLocked)
                {
                    // Note: New version of code does not have camera locking 
                    anim.SetFloat("horiMove", moveInputAction.ReadValue<Vector2>().x);
                    anim.SetFloat("vertMove", moveInputAction.ReadValue<Vector2>().y);
                    transform.forward = (zLockTarget.position - transform.position).normalized;
                }
                else
                {
                    if (!notMoving) transform.forward = Vector3.Lerp(transform.forward, direction, 0.1f);
                }

                Debug.Log(direction.magnitude);
                if (!notMoving)
                    rb.velocity = new Vector3(direction.x * totSpeed, rb.velocity.y, direction.z * totSpeed);
                else
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        } else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        rb.angularVelocity = Vector3.zero;

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        // Control animations
        anim.SetBool("Moving", Mathf.Abs(rb.velocity.x) > 0.05f || Mathf.Abs(rb.velocity.z) > 0.05f) ;
        anim.SetFloat("atkSpeed", attackSpeed);
        if (sprinting)
            anim.SetFloat("moveSpeed", moveSpeed * sprintModifier);
        else
            anim.SetFloat("moveSpeed", moveSpeed * chargeSpdMultiplier);

        // Control combo state
        /*
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
        */

        if (inputs.Player.Execute.triggered && RageManager.rage == 100)
        {
            // No longer doing execute attack
            // anim.Play("Execute");
        }

        if (attackInputAction.triggered)
        {
            /*
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
            */
            anim.SetBool("Combo", true);
        }
        if (jumpInputAction.triggered)
        {
            jumpDesired = true;
        }
        

        sprinting = sprintInputAction.IsPressed();
        /*
        if (sprintInputAction.triggered && cooldown <= 0)
        {
            anim.Play("Slide");
        }
        */
        

        // True or false based on if it's pressed the current frame
        if (attackInputAction.IsPressed())
        {
            if (!chargeSlowed) { chargeSpdMultiplier = .6f; chargeSlowed = true; }

            holdTime += Time.deltaTime;
            if (holdTime > holdTimeForCharge)
            {
                int old = charge;
                anim.SetBool("Charge", true);
                charge = (int)((holdTime - holdTimeForCharge) / timePerChargePhase);
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
        }
        else
        {
            if (chargeSlowed) { chargeSpdMultiplier = 1 ; chargeSlowed = false; }

            if (holdTime > holdTimeForCharge)
            {
                anim.SetBool("Charge", false);
                holdTime = 0;
            }
            else if (holdTime != 0)
            {
                holdTime = 0;
            }
        }


        chargeFX.SetInt("chargeLevel", charge);

    }

    private void FixedUpdate()
    {
        Debug.Log("onGround " + onGround);
        if (jumpDesired)
        {
            jumpDesired = false;
            if (onGround)
            {
                onGround = false;
                anim.SetTrigger("Jumping");
                rb.velocity += new Vector3(0, 5, 0) * Physics.gravity.magnitude / 9.8f;
            }
        }
        onGround = false;
    }
    public void AddCrystal(int type)
    {
        crystals[type]++;
        switch (type)
        {
            case 0:
            {
                    Color color = Color.red;
                    accumulateVFX.SetVector4("Color", color);
                    accumulateVFX.SendEvent("Accumulate");
                    break;
            }
            case 1:
            {
                    Color color = Color.green;
                    accumulateVFX.SetVector4("Color", color);
                    accumulateVFX.SendEvent("Accumulate");
                break;
            } 
            case 2:
            {
                    Color color = Color.blue;
                    accumulateVFX.SetVector4("Color", color);
                    accumulateVFX.SendEvent("Accumulate");
                    break;
            }
        }
        CalculateBuffs();
        HP = maxHP;
    }

    void CalculateBuffs()
    {
        int red = crystals[0];
        int green = crystals[1];
        int blue = crystals[2];

        // damageMult = (red - blue) * 0.2f + 1f;
        // attackSpeed = (blue - green) * 0.2f + 1f;
        // moveSpeed = (blue - green) * 0.2f + 1f;
        // healthBoost = (green - red) * 0.2f + 1f;
        damageMult = (red) * 0.2f + 1f;
        attackSpeed = (blue) * 0.2f + 1f;
        moveSpeed = (blue) * 0.2f + 1f;
        healthBoost = (green) * 0.2f + 1f;
        /*
        skillPointManager.SetAttack(damageMult);
        skillPointManager.SetSpeed(attackSpeed);
        skillPointManager.SetHP(healthBoost);
        */
    }

    void CheckForEnemies()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("EnemyHB"));
        Collider closest = null;
        float dist = Mathf.Infinity;
        foreach(Collider col in hit)
        {
            float mag = (col.gameObject.transform.position - transform.position).magnitude;
            if (mag < dist)
            {
                closest = col;
                dist = mag;
            }
        }
        if (closest != null)
        {
            closestEnemy = closest.gameObject.transform;
        }
    }

    IEnumerator Flash()
    {
        float str = 1f;  // tested magic numbers
        Color color = glowColors[charge];
        // Debug.Log(chargeGlow.GetFloat("_Strength"));
        chargeGlow.SetFloat("_Strength", str);
        chargeGlow.SetColor("_Color", color);
        while (str < 2.6f)
        {
            str = Mathf.Lerp(str, 3.8f, 0.02f);
            chargeGlow.SetFloat("_Strength", str);
            yield return new WaitForEndOfFrame();
        }
    }

    // Saves your eyes from a bunch of jank
    #region Animator Events

    public void ActivateSlash(int index)
    {
        slashVFX[index].SetActive(true);
        slashVFX[index].GetComponent<PlayerHitbox>().damage = (int)Mathf.Round(1 * damageMult);
    }

    public void DeactivateSlash(int index)
    {
        StartCoroutine(DeactivateWithDelay(slashVFX[index]));
    }

    IEnumerator DeactivateWithDelay(GameObject obj)
    {
        yield return new WaitForSeconds(0.12f);
        obj.SetActive(false);
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

    public void ChargeExecute()
    {
        StartCoroutine(YellowFlash());
    }

    IEnumerator YellowFlash()
    {
        float str = 1f;  // tested magic numbers
        Color color = glowColors[2];
        // Debug.Log(chargeGlow.GetFloat("_Strength"));
        chargeGlow.SetFloat("_Strength", str);
        chargeGlow.SetColor("_Color", color);
        while (str < 2.6f)
        {
            str = Mathf.Lerp(str, 3.8f, 0.02f);
            chargeGlow.SetFloat("_Strength", str);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SpawnBeam()
    {
        if (closestEnemy != null)
        {
            StartCoroutine(DespawnBeam(Instantiate(beamVFX, closestEnemy.position, Quaternion.identity)));
            closestEnemy.gameObject.GetComponent<Enemy>().Die(true);
            RageManager.SetRage(0);
        }
    }

    IEnumerator DespawnBeam(GameObject beam)
    {
        chargeGlow.SetColor("_Color", glowColors[0]);
        yield return new WaitForSeconds(2f);
        Destroy(beam);
    }

    public void StartSlide()
    {
        Vector3 direction = new Vector3(moveInputAction.ReadValue<Vector2>().x, 0, moveInputAction.ReadValue<Vector2>().y);
        rb.velocity = new Vector3(direction.x * slideSpeed, rb.velocity.y, direction.z * slideSpeed);
        sliding = true;
        invulnerable = true;
    }

    public void StopSlide()
    {
        sliding = false;
        invulnerable = false;
        rb.velocity = Vector3.zero;
        cooldown = slideCooldown;
    }

    public void SpawnSlash()
    {
        if (charge == 0) {
            slashVFX[5].SetActive(true);
            slashVFX[5].SetActive(false);
        }
        else if (charge == 1)
        {
            StartCoroutine(SpawnSlashes(2));
        }
        else if (charge == 2)
        {
            StartCoroutine(SpawnSlashes(4));
        } else
        {
            StartCoroutine(SpawnSlashes(8));
        }
        charge = 0;
        chargeGlow.SetColor("_Color", glowColors[0]);
    }

    IEnumerator SpawnSlashes(int n)
    {
        for (int i = 0; i < n; i++)
        {
            slashVFX[i].SetActive(true);
            yield return new WaitForSeconds(.09f);
        }
        for (int i = 0; i < n; i++)
        {
            slashVFX[i].SetActive(false);
        }
        if (n >= 8)
        {
            yield return new WaitForSeconds(.2f);
            for (int i = 0; i < n; i++)
            {
                slashVFX[i].SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
            for (int i = 0; i < n; i++)
            {
                slashVFX[i].SetActive(false);
            }
        }
    }

    public void SpawnStrike()
    {
        Vector3 pos = transform.position + Vector3.up * 0f;

        float damage = damageMult;
        float slowReduction = (attackSpeed - 1) * 0.1f;
        if (charge == 0) { }
        else if (charge == 1)
        {
            GameObject c = Instantiate(strikeVFX, pos, transform.rotation);
            GroundStrike strike = c.GetComponentInChildren<GroundStrike>();
            Attack atk = c.GetComponentInChildren<Attack>();
            strike.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atk.damage = Mathf.RoundToInt(30 * damageMult);
        } else if (charge < 3)
        {
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * 15));
            GameObject c = Instantiate(strikeVFX, pos, transform.rotation);
            GroundStrike strike = c.GetComponentInChildren<GroundStrike>();
            Attack atk = c.GetComponentInChildren<Attack>();
            strike.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atk.damage = Mathf.RoundToInt(40 * damageMult);

            GameObject l1 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, pos, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            GroundStrike strikel1 = l1.GetComponentInChildren<GroundStrike>();
            Attack atkl1 = l1.GetComponentInChildren<Attack>();
            strikel1.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkl1.damage = Mathf.RoundToInt(40 * damageMult);
            GroundStrike striker1 = r1.GetComponentInChildren<GroundStrike>();
            Attack atkr1 = r1.GetComponentInChildren<Attack>();
            striker1.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkr1.damage = Mathf.RoundToInt(40 * damageMult);
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 15, transform.rotation.eulerAngles.z));
            // Instantiate(strikeVFX, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * -15));
        } else
        {
            GameObject c = Instantiate(strikeVFX, pos, transform.rotation);
            GroundStrike strike = c.GetComponentInChildren<GroundStrike>();
            Attack atk = c.GetComponentInChildren<Attack>();
            strike.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atk.damage = Mathf.RoundToInt(50 * damageMult);

            GameObject l1 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r1 = Instantiate(strikeVFX, pos, transform.rotation);
            l1.transform.Rotate(Vector3.up, 15);
            r1.transform.Rotate(Vector3.up, -15);
            GroundStrike strikel1 = l1.GetComponentInChildren<GroundStrike>();
            Attack atkl1 = l1.GetComponentInChildren<Attack>();
            strikel1.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkl1.damage = Mathf.RoundToInt(40 * damageMult);
            GroundStrike striker1 = r1.GetComponentInChildren<GroundStrike>();
            Attack atkr1 = r1.GetComponentInChildren<Attack>();
            striker1.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkr1.damage = Mathf.RoundToInt(40 * damageMult);

            GameObject l2 = Instantiate(strikeVFX, pos, transform.rotation);
            GameObject r2 = Instantiate(strikeVFX, pos, transform.rotation);
            l2.transform.Rotate(Vector3.up, 30);
            r2.transform.Rotate(Vector3.up, -30);
            GroundStrike strikel2 = l2.GetComponentInChildren<GroundStrike>();
            Attack atkl2 = l2.GetComponentInChildren<Attack>();
            strikel2.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkl2.damage = Mathf.RoundToInt(40 * damageMult);
            GroundStrike striker2 = r2.GetComponentInChildren<GroundStrike>();
            Attack atkr2 = r2.GetComponentInChildren<Attack>();
            striker2.slowRate = Mathf.Max(strike.slowRate - slowReduction, 0.01f);
            atkr2.damage = Mathf.RoundToInt(40 * damageMult);
        }
        charge = 0;
        chargeGlow.SetColor("_Color", glowColors[0]);
    }


    #endregion
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(trfm.up, normal);
            float minGroundDotProduct = Mathf.Cos(25f * Mathf.Deg2Rad);
            if (upDot >= minGroundDotProduct)
            {
                onGround = true;
            }
        }
    }
}
