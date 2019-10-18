using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text timerText;

    public static GameManager Instance { get; private set; }
    public static GameState CurrentState { get; private set; }

    // components
    ControllerInputManager inputManager;

    // vals
    float survivalTime;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        CurrentState = GameState.startMenu;
        inputManager = GetComponent<ControllerInputManager>();

        enemySpawner?.SetEnabled(false);

        menuPanel.SetActive(true);
        connectionPanel.SetActive(false);
        gamePanel.SetActive(false);

        survivalTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case GameState.startMenu:
                if (inputManager.PressedStart())
                {
                    menuPanel.SetActive(false);
                    connectionPanel.SetActive(true);
                    CurrentState = GameState.connectionMenu;
                }
                break;
            case GameState.connectionMenu:
                if (inputManager.AllConnectedPlayersReady())
                {
                    connectionPanel.SetActive(false);
                    CurrentState = GameState.waitForPlayerReady;
                }
                break;
            case GameState.waitForPlayerReady:
                if (inputManager.AllPlayersReadyForEnemies())
                {
                    Debug.Log("starting game");
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
        timerText.text = (int)survivalTime + "s";
    }
}
