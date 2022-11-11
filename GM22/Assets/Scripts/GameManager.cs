using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gamePaused;
    [SerializeField] private PlayerController _playerControllerScr;
    public static PlayerController playerControllerScr;

    private void Awake()
    {
        playerControllerScr = _playerControllerScr;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            Pause();
        }
    }
    public static void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gamePaused = true;
        UIManager.EnableUI();
    }

    public static void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
        Time.timeScale = 1;
    }

    static Vector3 playerTargetOffset = new Vector3(0, 3, 0);
    public static void FacePlayer(Transform origin, bool usePitch = false)
    {
        Vector3 direction = (playerControllerScr.trfm.position + playerTargetOffset - origin.position).normalized; //direction vector from enemy to player
        Quaternion lookRotation;
        if (usePitch) { lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z)); } //target angle
        else { lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); }
        //for smooth rotation
        origin.rotation = Quaternion.Slerp(origin.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
