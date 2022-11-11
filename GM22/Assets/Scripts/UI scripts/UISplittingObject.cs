using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplittingObject : UIObj
{
    [SerializeField] GameObject[] objs;
    [SerializeField] SpriteRenderer rend;

    private new void OnEnable()
    {
        base.OnEnable();

        for (int i = 0; i < 2; i++)
        {
            objs[i].SetActive(false);
            objs[i].transform.localPosition = Vector3.zero;
        }
        rend.enabled = true;
    }

    private new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && isTouching)
        {
            doSlash();
            objs[0].SetActive(true);
            objs[1].SetActive(true);
            rend.enabled = false;
        }
    }
}
