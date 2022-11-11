using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageManager : MonoBehaviour
{
    [SerializeField] Transform PwrScalerTrfm;
    Vector3 scale = Vector3.one;
    static RageManager self;
    int lastVal;
    public static int rage;
    private void Start()
    {
        lastVal = rage;
        self = GetComponent<RageManager>();
    }

    public static void AddRage(int amount)
    {
        rage += amount;
        self.scale.x = ((float)amount) / 100;
        self.PwrScalerTrfm.localScale = self.scale;
    }
    public static void SetRage(int amount)
    {
        self.scale.x = ((float)amount) / 100;
        self.PwrScalerTrfm.localScale = self.scale;
    }

}
