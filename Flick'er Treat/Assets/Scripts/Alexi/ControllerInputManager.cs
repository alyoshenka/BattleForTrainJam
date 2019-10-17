﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// connects controllers to players
/// </summary>
public class ControllerInputManager : MonoBehaviour
{
    public static int maxPlayerCount = 4; // maximum number of players allowed

    [Tooltip("Controllable characters")] public CharacterController[] characters;
    [Tooltip("Disable players without controllers?")] public bool areUnusedDisabled = true;

    static List<CharacterController> activePlayers;
    
    // Start is called before the first frame update
    void Start()
    {
        int maxChar;
        if(characters.Length > maxPlayerCount)
        {
            Debug.LogWarning("There are more characters than there are controllers");
            maxChar = maxPlayerCount;
            if (areUnusedDisabled)
            {
                for(int i = maxPlayerCount - 1; i < characters.Length; i++)
                {
                    characters[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            maxChar = characters.Length;
        }

        for(int i = 0; i < maxChar; i++)
        {
            characters[i].playerID = i;
        }

        if(null == activePlayers) { activePlayers = new List<CharacterController>(); }
    }

    public bool PressedStart()
    {
        foreach(CharacterController cc in characters)
        {
            if (ReInput.players.GetPlayer(cc.playerID).GetAnyButtonDown()) { return true; }
        }
        return false;
    }

    void CheckForConnections()
    {
        foreach (CharacterController cc in characters)
        {
            if (!cc.isConnected)
            {
                if (ReInput.players.GetPlayer(cc.playerID).GetButtonDown("Connect"))
                {
                    cc.gameObject.SetActive(true);
                    cc.isConnected = true;
                    cc.ConnectUI();
                    activePlayers.Add(cc);

                    Debug.Log(cc.name + " connected");
                }
            }
        }
    }

    public bool AllConnectedPlayersReady()
    {
        CheckForConnections();

        foreach (CharacterController cc in activePlayers)
        {
            if (!cc.isReady)
            {
                if (ReInput.players.GetPlayer(cc.playerID).GetButtonDown("Ready"))
                {
                    cc.isReady = true;
                    Debug.Log(cc.name + " ready");
                    cc.ReadyUI();
                }
                else { return false; }
            }
        }
        if(activePlayers.Count < 1) { return false; }

        return true;
    }

    public bool AllPlayersReadyForEnemies()
    {
        foreach(CharacterController cc in activePlayers)
        {
            if (!cc.hasTurnedOnFlashlight) { return false; }
        }
        return true;
    }

    public static void AddActivePlayer(CharacterController newChar)
    {
        if (!activePlayers.Contains(newChar))
        {
            activePlayers.Add(newChar);
            if (activePlayers.Count >= maxPlayerCount)
            {
                Debug.LogError("too many players");
            }
        }
    }
}
