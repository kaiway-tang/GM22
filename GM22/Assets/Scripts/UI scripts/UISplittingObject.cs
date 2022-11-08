using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplittingObject : UIObj
{
    [SerializeField] GameObject[] objs; //0: slash
    [SerializeField] Transform trfm;
    [SerializeField] Vector2 offset;

    private new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && isTouching)
        {
            doSlash();
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void doSlash()
    {
        Transform slashFX = Instantiate(objs[0], trfm.position + trfm.right * offset.x + trfm.up * offset.y, trfm.rotation).transform;
        slashFX.parent = UIParent;
        slashFX.Rotate(Vector3.forward * 15);
    }

    private new void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<UIObj>().objID == cursor)
        {
            base.OnTriggerEnter(col);
        }
    }

    private new void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<UIObj>().objID == cursor)
        {
            base.OnTriggerExit(col);
        }
    }
}
