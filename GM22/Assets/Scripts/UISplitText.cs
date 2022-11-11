using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplitText : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Color colChange;
    [SerializeField] float spd;
    void OnDisable()
    {
        rend.color = Color.white;
    }

    private void OnEnable()
    {
        StartCoroutine(unscaledFU());
    }

    IEnumerator unscaledFU()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(.02f);

            rend.color -= colChange;
            trfm.position += trfm.right * spd;
        }
    }
}
