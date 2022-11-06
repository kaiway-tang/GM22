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

    [Tooltip("Amount of time in seconds before a slash combo is reset.")]
    [SerializeField] float comboWindow = 2f;
    [SerializeField] float comboCountdown = 0f;
    [SerializeField] int comboNum = 0;
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
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            bool notMoving = direction.magnitude == 0;

            direction = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward).normalized;

            // Always face direction of movement
            // if (!notMoving) transform.forward = Vector3.Lerp(transform.forward, direction, 0.6f);

            // Assume no vertical movement/gravity as of rn (so y velocity ALWAYS 0)
            if (!notMoving)
                rb.velocity = direction * speed;
            else
                rb.velocity = Vector3.zero;
        } else
        {
            rb.velocity = Vector3.zero;
        }

        // Control movement animation
        anim.SetBool("Moving", rb.velocity.magnitude != 0);

        if (Input.GetMouseButton(0))
        {
            // anim.Play("2-Hand Swing0");
            // Attack();
            anim.SetBool("Combo", true);
        }
            

        // Timers
        if (comboCountdown > 0 && !swinging)
        {
            comboCountdown -= Time.deltaTime;
        } 

        if (comboCountdown <= 0)
        {
            comboCountdown = 0;
            comboNum = 0;
        }
    }

    void Attack()
    {
        // If already swinging, don't do anything
        if (!swinging)
        {

            // Use a counter to dictate which swing to use
            // Start a timer to count down. When the timer runs out, the counter resets
            if (comboCountdown <= 0)
            {
                
            }
            else
            {
                comboNum++;
                //if (comboNum > 2) // Combo resets 
                //{
                //    comboNum = 0;
                //}
            }

            // Increment the counter when a swing happens, and reset the timer
            StartCoroutine("Swing");

            // Jump to each swing's mechanics based on the counter
        }
    }

    IEnumerator Swing()
    {
        // Maybe a coroutine? Prob a good idea tbh
        Debug.Log("Firing");
        // Set flag to start swing 
        swinging = true;
        // Start animation
        anim.Play("2-Hand Swing" + comboNum);
        // wait(?)
        // Activate collider
        // Wait
        float len = anim.GetCurrentAnimatorStateInfo(0).length - anim.GetNextAnimatorStateInfo(0).length;
        Debug.Log("Time: " + len);

        // yield return new WaitForSeconds(len);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Idle"));
        // Deactivate collider
        // Wait for end lag 
        yield return new WaitForSeconds(0.2f);
        // Unset flag
        swinging = false;
        if (comboNum == 2)
        {
            comboNum = 0;
            comboCountdown = 0;
            // anim.Play("Idle");
        } else 
            comboCountdown = comboWindow;
        // yield return new WaitForEndOfFrame();
    }

    public void ResetCombo()
    {
        anim.SetBool("Combo", false);
    }


    public void Readjust()
    {
        transform.position += new Vector3(animObject.transform.localPosition.x, 0, animObject.transform.localPosition.z);
    }
}
