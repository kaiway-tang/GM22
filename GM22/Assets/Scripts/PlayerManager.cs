using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    //FOR SOME REASON POSITION IN ARMATURE UPDATES BUT NOT THE ACC CHARACTER THING

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion


    //public game object that references player
    public GameObject player;
}
