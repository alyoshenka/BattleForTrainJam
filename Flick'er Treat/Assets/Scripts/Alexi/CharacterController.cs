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
    [Header("Components")]
    [Tooltip("Flasklight object")] public GameObject flashlight;
    public CharacterSelection connectionScreen;

    [Header("Controls")]
    [Tooltip("Movement force - flashlight ON")] public float movementForceOn;
    [Tooltip("Movement force - flashlight OFF")] public float movementForceOff;

    [Header("Cosmetics")]
    [Tooltip("How fast the player rotates")] [Range(0.01f, 0.5f)] public float rotationLerp = 0.2f;
    [Tooltip("Minimum velocity magnitude to rotate")] public float minimumVelocityRotationThreshold = 0.1f;

    [Tooltip("Rewired ID")]
    public int playerID;

    // components
    Player player; // rewired player
    Rigidbody myRigidbody;

    // states
    bool isFlashlightOn;
    public bool hasTurnedOnFlashlight { get; private set; }
    public bool isConnected { get; set; }
    public bool isReady { get; set; }

    public bool ThisIsNotTheFinalScene = true;

    // holders
    Vector2 movementInput, movementVector;
    Quaternion targetRotation;
    float movementForce;

    // Start is called before the first frame update
    void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
        myRigidbody = GetComponent<Rigidbody>();

        movementInput = Vector2.zero;
        targetRotation = Quaternion.identity;

        isConnected = isReady = false;

        TurnOffFlashlight();

        gameObject.SetActive(ThisIsNotTheFinalScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) { return; }

        GetInput();
        Move();
        if (movementInput.magnitude > minimumVelocityRotationThreshold) { Rotate(); }
    }

    /// <summary>
    /// get controller input
    /// </summary>
    void GetInput()
    {
        movementInput = new Vector2(player.GetAxis("MoveHorizontal"), player.GetAxis("MoveVertical")).normalized;

        if (player.GetButtonDown("Flashlight"))
        {
            TurnOnFlashlight();
        }
        else if (player.GetButtonUp("Flashlight"))
        {
            TurnOffFlashlight();
        }
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
        float targetAngle = Mathf.Atan2(-movementInput.x, -movementInput.y) * Mathf.Rad2Deg;
        float currentAngle = transform.rotation.eulerAngles.y;
        if(currentAngle - targetAngle > 180) { currentAngle -= 360; }
        float rotation = Mathf.Lerp(currentAngle, targetAngle, rotationLerp);
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    void TurnOnFlashlight()
    {
        movementForce = movementForceOn;
        isFlashlightOn = true;
        hasTurnedOnFlashlight = true;

        flashlight.SetActive(true);
    }

    void TurnOffFlashlight()
    {
        movementForce = movementForceOff;
        isFlashlightOn = false;

        flashlight.SetActive(false);
    }

    public void ConnectUI()
    {
        connectionScreen.Connect();
    }

    public void ReadyUI()
    {
        connectionScreen.Ready();
    }
}
