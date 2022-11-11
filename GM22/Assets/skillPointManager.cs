using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillPointManager : MonoBehaviour
{
    [SerializeField] Transform[] skillPoints; //0: attack; 1: hp; 2: speed
    [SerializeField] Vector3[] move;

    void Start()
    {
        move[0] = new Vector3(-.866f,0.5f,0);
        move[1] = new Vector3(.866f,0.5f,0);
        move[2] = new Vector3(0,-1,0);
    }

    public void SetAttack(float i)
    {
        ProcessIncrement(0, i);
    }
    public void SetHP(float i)
    {
        ProcessIncrement(1, i);
    }
    public void SetSpeed(float i)
    {
        ProcessIncrement(2, i);
    }

    void ProcessIncrement(int ID, float i)
    {
        skillPoints[ID].localPosition = move[ID] * i * 0.5f;
    }
}
