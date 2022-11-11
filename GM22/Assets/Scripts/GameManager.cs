using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gamePaused;

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
}