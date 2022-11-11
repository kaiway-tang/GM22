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

    public void IncrementAttack()
    {
        ProcessIncrement(0);
    }
    public void IncrementHP()
    {
        ProcessIncrement(1);
    }
    public void IncrementSpeed()
    {
        ProcessIncrement(2);
    }

    void ProcessIncrement(int ID)
    {
        skillPoints[ID].localPosition += move[ID] * .2f;
        //pointLines[0].position = skillPoints[1].position - skillPoints[0].position;
        //pointLines[1].position = skillPoints[2].position - skillPoints[1].position;
        //pointLines[2].position = skillPoints[0].position - skillPoints[2].position;
    }
}
