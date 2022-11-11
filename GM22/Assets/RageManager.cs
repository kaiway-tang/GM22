using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageManager : MonoBehaviour
{
    [SerializeField] Transform PwrScalerTrfm;
    Vector3 scale = Vector3.one;
    static RageManager self;
    public static int rage;
    private void Start()
    {
        self = GetComponent<RageManager>();
        SetRage(0);
    }

    public static void AddRage(int amount)
    {
        rage += amount;
        if (rage < 0) { rage = 0; }
        if (rage > 100) { rage = 100; }
        self.scale.x = ((float)rage) / 100;
        self.PwrScalerTrfm.localScale = self.scale;
    }
    public static void SetRage(int amount)
    {
        rage = amount;
        self.scale.x = ((float)rage) / 100;
        self.PwrScalerTrfm.localScale = self.scale;
    }

}
