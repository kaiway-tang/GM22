using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorObj : UIObj
{
    public static Transform trfm;
    [SerializeField] Camera mainCam;
    Vector3 mousePos;
    void Start()
    {
        trfm = transform;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePos.z = mainCam.nearClipPlane + .1f;
        trfm.position = mainCam.ScreenToWorldPoint(mousePos);
    }
}
