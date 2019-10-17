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

    [Header("Cosmetics")]
    [Tooltip("How fast the player rotates")] [Range(0.01f, 0.5f)] public float rotationLerp = 0.2f;

    [Tooltip("Rewired ID")]
    public int playerID;

    // components
    Player player; // rewired player
    Rigidbody myRigidbody;

    // holders
    Vector2 movementInput, movementVector;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        myRigidbody = GetComponent<Rigidbody>();

        movementInput = Vector2.zero;
        targetRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Rotate();
    }

    /// <summary>
    /// get controller input
    /// </summary>
    void GetInput()
    {
        movementInput = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical")).normalized;
    }

    /// <summary>
    /// apply the input
    /// </summary>
    void Move()
    {
        if (movementInput.x > 0) { myRigidbody.AddForce(Vector3.right * movementForce); }
        else if (movementInput.x < 0) { myRigidbody.AddForce(Vector3.left * movementForce); }
        if (movementInput.y > 0) { myRigidbody.AddForce(Vector3.forward * movementForce); }
        else if (movementInput.y < 0) { myRigidbody.AddForce(Vector3.back * movementForce); }
    }

    /// <summary>
    /// rotate with respect to velocity
    /// </summary>
    void Rotate()
    {
        float targetAngle = Mathf.Atan2(myRigidbody.velocity.x, myRigidbody.velocity.z) * Mathf.Rad2Deg;
        float currentAngle = transform.rotation.eulerAngles.y;
        if(currentAngle - targetAngle > 180) { currentAngle -= 360; }
        float rotation = Mathf.Lerp(currentAngle, targetAngle, rotationLerp);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
}
