using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorObj : UIObj
{
    [SerializeField] Transform trfm;
    [SerializeField] Camera mainCam;
    Vector3 mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePos.z = mainCam.nearClipPlane + .01f;
        trfm.position = mainCam.ScreenToWorldPoint(mousePos);
    }
}
