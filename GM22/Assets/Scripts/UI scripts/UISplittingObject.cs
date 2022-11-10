using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplittingObject : UIObj
{
    [SerializeField] GameObject[] objs; //0: slash
    [SerializeField] Vector2 offset;
    [SerializeField] SpriteRenderer rend;

    private new void OnEnable()
    {
        base.OnEnable();

        for (int i = 1; i < 3; i++)
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
            objs[1].SetActive(true);
            objs[2].SetActive(true);
            rend.enabled = false;
        }
    }

    void doSlash()
    {
        Transform slashFX = Instantiate(objs[0], trfm.position + trfm.right * offset.x + trfm.up * offset.y, trfm.rotation).transform;
        slashFX.parent = UIParent;
        slashFX.Rotate(Vector3.forward * 15);
    }
}
