using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startButton : UIObj
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && isTouching)
        {
            doSlash();
            Invoke("start",.2f);
        }
    }

    void start()
    {
        SceneManager.LoadScene("mainScene");
    }
}
