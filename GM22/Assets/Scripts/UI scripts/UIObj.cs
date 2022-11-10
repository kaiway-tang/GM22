using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObj : MonoBehaviour
{
    public int objID;
    protected const int cursor = 1 , quit = 2, resume = 3;
    [SerializeField] UIManager UIManagerScript;
    [SerializeField] protected Transform UIParent, bracket;
    [SerializeField] protected bool isTouching, touchingUpdated, adjust, doDisable;
    [SerializeField] Vector2 dimensions; //halved dimensions
    [SerializeField] protected Transform trfm;
    protected void OnEnable()
    {
        touchingUpdated = isTouching;
        adjust = true;
        doDisable = false;

        StartCoroutine(unscaledFU());
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTouching)
        {
            adjust = true;
            doDisable = true;

            if (objID == resume)
            {
                GameManager.Resume();
                UIManagerScript.disable(10);
            } else if (objID == quit)
            {
                Application.Quit();
            }
        }
    }

    IEnumerator unscaledFU()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(.02f);

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

            isTouching = (Mathf.Abs(cursorObj.trfm.localPosition.x - trfm.localPosition.x) < dimensions.x && Mathf.Abs(cursorObj.trfm.localPosition.y - trfm.localPosition.y) < dimensions.y) && !doDisable;
            if (touchingUpdated != isTouching)
            {
                touchingUpdated = isTouching;
                adjust = true;
            }
        }
    }
}
