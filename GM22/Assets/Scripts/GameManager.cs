using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gamePaused;
    [SerializeField] private PlayerController _playerControllerScr;
    public static PlayerController playerControllerScr;
    [SerializeField] private Transform _camTrfm;
    public static Transform camTrfm;
    public static GameManager self;
    bool playerIsDead;

    [SerializeField] private EnemySpawner[] spawners;
    EnemySpawner curSpawner;
    private int wavesCleared;

    private void Awake()
    {
        self = GetComponent<GameManager>();
        playerControllerScr = _playerControllerScr;
        camTrfm = _camTrfm;
    }

    private void Update()
    {
        if (playerIsDead && Input.GetKeyDown(KeyCode.R))
        {
            playerIsDead = false;
            gamePaused = false;
            SceneManager.LoadScene("mainScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            Pause();
        }

        if (!gamePaused)
        {
            SpawnWaves();
        }
    }

    public void PlayerDied()
    {
        playerIsDead = true;
        transform.parent = null;
    }

    void SpawnWaves()
    {
        if (curSpawner == null || curSpawner.enemies.Count == 0)
        {
            int index = Mathf.RoundToInt(Random.Range(0, spawners.Length));
            curSpawner = spawners[index];
            curSpawner.Spawn();
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

    public static void FaceCamera(Transform origin)
    {
        Vector3 direction = (camTrfm.position - origin.position).normalized; //direction vector from enemy to player
        Quaternion lookRotation;
        lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        //for smooth rotation
        origin.rotation = Quaternion.Slerp(origin.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
