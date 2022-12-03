using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm;
    [SerializeField] float returnRate, intensity;
    Vector3 rotation;
    static int trauma;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { addTrauma(10); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { addTrauma(20); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { addTrauma(40); }
    }
    void FixedUpdate()
    {
        returnRate++;
        cameraTrfm.localEulerAngles += new Vector3(5,0,0);
        return;
        if (trauma > 0)
        {
            trauma--;
            rotation.x = Random.Range(0,2) * 2 - 1;
            rotation.y = Random.Range(0,2) * 2 - 1;
            rotation.z = Random.Range(0,2) * 2 - 1;
            rotation *= intensity;
            cameraTrfm.localEulerAngles += rotation;
        }
        rotation.x = cameraTrfm.localEulerAngles.x * returnRate;
        rotation.y = cameraTrfm.localEulerAngles.y * returnRate;
        rotation.z = cameraTrfm.localEulerAngles.z * returnRate;
    }

    public static void addTrauma(int amount)
    {
        trauma += amount;
    }
}
