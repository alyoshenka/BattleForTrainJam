using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// manages character movement and input handling
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("Movement force")] public float movementForce;

    [Tooltip("Rewired ID")]
    public int playerID;

    // components
    Player player; // rewired player
    Rigidbody myRigidbody;

    // holders
    Vector2 movementInput, movementVector;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        myRigidbody = GetComponent<Rigidbody>();

        movementInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical"));
        if (movementInput.x > 0) { myRigidbody.AddForce(Vector3.right * movementForce); }
        else if (movementInput.x < 0) { myRigidbody.AddForce(Vector3.left * movementForce); }
        if (movementInput.y > 0) { myRigidbody.AddForce(Vector3.forward * movementForce); }
        else if (movementInput.y < 0) { myRigidbody.AddForce(Vector3.back * movementForce); }
    }
}
