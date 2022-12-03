using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrab : MonoBehaviour
{
    [SerializeField] private Enemy[] controllers;

    [SerializeField] private MeshRenderer eye;
    [SerializeField] private MeshRenderer antenna;

    [SerializeField] private Material[] colors;

    [SerializeField] private RuntimeAnimatorController[] animControllers;

    private int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var controller in controllers)
        {
            controller.enabled = false;
        }
        
        controllers[num].enabled = true;
        antenna.material = colors[num];
        eye.material = colors[num];
        GetComponent<Animator>().runtimeAnimatorController = num == 2 ? animControllers[1] : animControllers[0];
        StartCoroutine(chooseRandom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator chooseRandom()
    {
        yield return new WaitForSeconds(10);
        foreach (var controller in controllers)
        {
            controller.enabled = false;
        }

        num++;
        if (num > 2) num = 0;
        controllers[num].enabled = true;
        antenna.material = colors[num];
        eye.material = colors[num];
        GetComponent<Animator>().runtimeAnimatorController = num == 2 ? animControllers[1] : animControllers[0];
        StartCoroutine(chooseRandom());
    }
}
