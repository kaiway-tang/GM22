using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] HPEntity HPEntityScr;
    [SerializeField] Transform HPBarTrfm, HPScalerTrfm;
    [SerializeField] SpriteRenderer[] rend;
    [SerializeField] Color alphaChange;
    Vector3 scale = Vector3.one;
    int lastHP;
    bool active;

    private void Start()
    {
        lastHP = HPEntityScr.HP;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GameManager.FaceCamera(HPBarTrfm);

        if (lastHP != HPEntityScr.HP)
        {
            lastHP = HPEntityScr.HP;
            scale.x = ((float)lastHP) / HPEntityScr.maxHP;
            HPScalerTrfm.localScale = scale;
            active = true;
            rend[0].color = Color.white;
            rend[1].color = Color.white;
        }
        if (active)
        {
            if (rend[0].color.a > 0)
            {
                rend[0].color -= alphaChange;
                rend[1].color -= alphaChange;
            }
            else
            {
                rend[0].color = new Color(1,1,1,0);
                rend[1].color = new Color(1,1,1,0);
                active = false;
            }
        }
    }
}
