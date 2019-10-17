using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo
// change from category to layout

/// <summary>
/// connects controllers to players
/// </summary>
public class ControllerInputManager : MonoBehaviour
{
    const int maxPlayerCount = 4; // maximum number of players allowed

    [Tooltip("Controllable characters")] public CharacterController[] characters;
    [Tooltip("Disable players without controllers?")] public bool areUnusedDisabled = true;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
