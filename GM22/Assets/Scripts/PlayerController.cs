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
    float comboCountdown = 0f;
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

        // Using isometric to test, will change later
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
        // Always face direction of movement
        if (direction != Vector3.zero) transform.forward = Vector3.Lerp(transform.forward, direction, 0.6f);

        // Assume no vertical movement/gravity as of rn (so y velocity ALWAYS 0)
        rb.velocity = direction * speed;

        // Control movement animation
        anim.SetBool("Moving", rb.velocity.magnitude != 0);

        if (Input.GetMouseButton(0))
        {
            // anim.Play("2-Hand Swing0");
            Attack();
        }
            

        // Timers
        if (comboCountdown > 0 && !swinging)
        {
            comboCountdown -= Time.deltaTime;
        }
    }

    void Attack()
    {
        // If already swinging, don't do anything
        if (swinging)
            return;

        // Use a counter to dictate which swing to use
        // Start a timer to count down. When the timer runs out, the counter resets
        if (comboCountdown <= 0)
        {
            comboCountdown = 0;
            comboNum = 0;
        } else
        {
            comboNum++;
            if (comboNum > 2) // Combo resets 
            {
                comboNum = 0;
            }
        }

        // Increment the counter when a swing happens, and reset the timer
        StartCoroutine("Swing");

        // Jump to each swing's mechanics based on the counter
    }

    IEnumerator Swing()
    {
        // Maybe a coroutine? Prob a good idea tbh

        // Set flag to start swing 
        swinging = true;
        // Start animation
        anim.Play("2-Hand Swing" + comboNum);
        // wait(?)
        // Activate collider
        // Wait
        float len = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Debug.Log("Time: " + len);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Base.Idle"));
        // Deactivate collider
        // Wait for end lag 
        yield return new WaitForSeconds(0.5f);
        // Unset flag
        swinging = false;
        comboCountdown = comboWindow;
        // yield return new WaitForEndOfFrame();
    }

    public void Readjust()
    {
        transform.position += new Vector3(animObject.transform.localPosition.x, 0, animObject.transform.localPosition.z);
    }
}
