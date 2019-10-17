using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Game state variables")]
    [Tooltip("Timer for starting game")] public int startTimer;

    [Header("Menu navigation")]
    public GameObject menuPanel;
    public GameObject connectionPanel;

    public static GameManager Instance { get; private set; }
    public static GameState CurrentState { get; private set; }
    ControllerInputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        CurrentState = GameState.startMenu;
        inputManager = GetComponent<ControllerInputManager>();
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
                    // start game
                    Debug.Log("starting game");
                }
                break;
            case GameState.inGame:
                break;
            default:
                Debug.LogError("Invalid game state");
                break;
        }
    }

    #region StateUpdates

    void UpdateStart()
    {
        if (AllPlayersHaveTurnedOnFlashlights())
        {
            StartGameState();
        }
    }

    #endregion

    #region StateChangeChecks

    bool AllPlayersHaveTurnedOnFlashlights()
    {
        return false;
    }

    #endregion

    #region StateTransitions

    void StartGameState()
    {
        Debug.Log("starting game");

        // timer start overlay

        // at end -> CadeThing.SendOutEnemies();
    }

    #endregion

    
}
