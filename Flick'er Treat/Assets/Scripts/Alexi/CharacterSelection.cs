using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject selection;
    public GameObject ready;

    void Start()
    {
        selection.SetActive(false);
        ready.SetActive(false);
    }

    public void Connect()
    {
        // cool animation here

        selection.SetActive(true);
    }

    public void Ready()
    {
        // cool animation here

        ready.SetActive(true);
    }
}
