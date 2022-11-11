using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class YCamera : MonoBehaviour
{
    [SerializeField] private GameObject NorCam;
    [SerializeField] private GameObject ZTargetCam;
    
    // Start is called before the first frame update
    void Start()
    {
        NorCam.SetActive(true);
        ZTargetCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZTarget(GameObject g)
    {
        if (g != null)
        {
            ZTargetCam.GetComponent<CinemachineVirtualCamera>().LookAt = g.transform;
            ZTargetCam.SetActive(true);
            NorCam.SetActive(false);
        }
    }

    public void Normal()
    {
        ZTargetCam.SetActive(false);
        NorCam.SetActive(true);
    }
}
