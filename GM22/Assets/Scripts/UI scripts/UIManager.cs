using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIParent;
    static UIManager UIManScr;
    int disableDelay;

    private void Start()
    {
        UIManScr = GetComponent<UIManager>();
    }

    void OnEnable()
    {
        StartCoroutine(unscaledFU());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator unscaledFU()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(.02f);

            if (disableDelay > 0)
            {
                disableDelay--;
                if (disableDelay == 0)
                {
                    UIParent.SetActive(false);
                }
            }
        }
    }

    public static void EnableUI()
    {
        UIManScr.UIParent.SetActive(true);
    }

    public void disable(int delay) //delay in ticks
    {
        if (delay == 0)
        {
            UIParent.SetActive(false);
            return;
        }
        disableDelay = delay;
    }
}
