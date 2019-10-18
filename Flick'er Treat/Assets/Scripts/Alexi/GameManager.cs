using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ControllerInputManager))]
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        startMenu,
        connectionMenu,
        waitForPlayerReady,
        inGame,
        gameOver,
        highScoreInput
    };

    [Header("Components and References")]
    public EnemySpawning enemySpawner;

    [Header("Game state variables")]
    [Tooltip("Timer for starting game")] public int startTimer;

    [Header("Menu navigation")]
    public GameObject menuPanel;
    public GameObject connectionPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject creditsPanel;
    public GameObject controlsPanel;
    public Text timerText;

    [Header("Audio")]
    public AudioClip flashlightClick;
    public AudioClip grass;
    public float grassVolume;

    public static GameManager Instance { get; private set; }
    public static GameState CurrentState { get; private set; }
    public static bool isPaused { get; private set; }

    // components
    ControllerInputManager inputManager;

    // vals
    float survivalTime;
    bool inMainMenu;

    void Awake()
    {
        Instance = this;

        CurrentState = GameState.startMenu;
        inputManager = GetComponent<ControllerInputManager>();

        enemySpawner?.SetEnabled(false);

        menuPanel.SetActive(true);
        connectionPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);

        survivalTime = 0;
        isPaused = false;
        inMainMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case GameState.startMenu:
                if (inMainMenu)
                {
                    if (inputManager.ToCredits())
                    {

                    }
                    else if (inputManager.ToControls())
                    {


                    }
                    else if (inputManager.PressedStart())
                    {
                        menuPanel.SetActive(false);
                        connectionPanel.SetActive(true);
                        CurrentState = GameState.connectionMenu;
                    }
                }
                else
                {

                }
               
                
                break;
            case GameState.connectionMenu:
                if (inputManager.AllConnectedPlayersReady())
                {
                    connectionPanel.SetActive(false);
                    CurrentState = GameState.waitForPlayerReady;
                    gamePanel.SetActive(true);
                }
                break;
            case GameState.waitForPlayerReady:
                if (inputManager.AllPlayersReadyForEnemies())
                {
                    CurrentState = GameState.inGame;
                    enemySpawner.SetEnabled(true);
                    enemySpawner.spawnEnemies(2, 17);
                    gamePanel.SetActive(true);
                }
                break;
            case GameState.inGame:
                UpdateGame();
                break;
            case GameState.gameOver:
                break;
            default:
                Debug.LogError("Invalid game state");
                break;
        }
    }

    void UpdateGame()
    {
        survivalTime += Time.deltaTime;
        timerText.text = "Survival time: " + (int)survivalTime;

        if (isPaused)
        {
            if (inputManager.CheckForPause())
            {
                UnPause();
            }
            if (inputManager.CheckForMenu())
            {
                Menu();
            }
        }
        else
        {
            if (inputManager.CheckForPause())
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Debug.Log("pause");
    }

    void UnPause()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Debug.Log("unpause");
    }

    void Menu()
    {
        isPaused = false;
        foreach(CharacterController cc in FindObjectsOfType<CharacterController>()) { cc.isConnected = false; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("menu");
    }

}
