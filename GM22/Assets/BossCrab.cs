using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrab : MonoBehaviour
{
    [SerializeField] private Enemy[] controllers;

    [SerializeField] private MeshRenderer eye;
    [SerializeField] private MeshRenderer antenna;

    [SerializeField] private Material[] colors;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var controller in controllers)
        {
            controller.enabled = false;
        }

        int num = Random.Range(0, controllers.Length);
        controllers[num].enabled = true;
        antenna.material = colors[num];
        eye.material = colors[num];
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

        int num = Random.Range(0, controllers.Length);
        controllers[num].enabled = true;
        antenna.material = colors[num];
        eye.material = colors[num];
        StartCoroutine(chooseRandom());
    }
}
