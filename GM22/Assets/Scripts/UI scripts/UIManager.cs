using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIParent;
    int disableDelay;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            enable();
        }
    }
    private void FixedUpdate()
    {
        if (disableDelay > 0)
        {
            disableDelay--;
            if (disableDelay == 0)
            {
                UIParent.SetActive(false);
            }
        }
    }
    public void enable()
    {
        UIParent.SetActive(true);
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
