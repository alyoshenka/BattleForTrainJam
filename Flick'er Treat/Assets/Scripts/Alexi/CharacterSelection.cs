using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject connect;
    public GameObject selection;
    public GameObject ready;

    void Awake()
    {
        connect.SetActive(true);
        selection.SetActive(false);
        ready.SetActive(false);
    }

    public void Connect()
    {
        // cool animation here
        connect.SetActive(false);
        selection.SetActive(true);
    }

    public void Ready()
    {
        // cool animation here
        selection.SetActive(false);
        ready.SetActive(true);
    }
}
