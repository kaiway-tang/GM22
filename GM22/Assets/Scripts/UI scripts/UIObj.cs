using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObj : MonoBehaviour
{
    public int objID;
    protected const int cursor = 1 , quit = 2, resume = 3;
    [SerializeField] UIManager UIManagerScript;
    [SerializeField] protected Transform UIParent, bracket;
    [SerializeField] protected bool isTouching, adjust;
    void Start()
    {
        
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTouching)
        {
            if (objID == 3)
            {
                UIManagerScript.disable(10);
            }
        }
    }

    protected void FixedUpdate()
    {
        if (objID > 1)
        {
            if (isTouching)
            {
                if (adjust && bracket.localScale.y < .6f)
                {
                    bracket.localScale += new Vector3(0, 0.09f, 0);
                }
                else
                {
                    bracket.localScale = new Vector3(0.5f, .6f, 0);
                    adjust = false;
                }
            }
            else
            {
                if (adjust && bracket.localScale.y > 0)
                {
                    bracket.localScale -= new Vector3(0, 0.09f, 0);
                }
                else
                {
                    bracket.localScale = new Vector3(0.5f, 0f, 0);
                    adjust = false;
                }
            }
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<UIObj>().objID == cursor)
        {
            isTouching = true;
            adjust = true;
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<UIObj>().objID == cursor)
        {
            isTouching = false;
            adjust = true;
        }
    }
}
