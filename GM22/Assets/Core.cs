using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    Rigidbody rigidbody;

    public Material mt;
    public Color32[] colors;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        //rainbow effect
        mt = gameObject.GetComponentInChildren<MeshRenderer>().material;
        colors = new Color32[7]
        {
            new Color32(0, 245, 255, 255), //sky blue
            new Color32(0, 155, 255, 255), //dark blue
            new Color32(0, 17, 113, 255), // super dark blue
            new Color32(0, 6, 39, 255), // almost black
            new Color32(0, 17, 113, 255), // super dark blue
            new Color32(0, 155, 255, 255), //dark blue
            new Color32(0, 245, 255, 255), //sky blue

        };
        StartCoroutine(Cycle());
    }

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        float z = Random.Range(-0.1f, 0.1f);
        rigidbody.AddForce(new Vector3(x, y, z));
    }

    public IEnumerator Cycle()
    {
        int i = 0;
        while (true)
        {
            for (float lerper = 0f; lerper < 1f; lerper += 0.001f)
            {
                mt.color = Color.Lerp(colors[i % 7], colors[(i + 1) % 7], lerper);
                yield return null;
            }
            i++;
        }
    }
}

