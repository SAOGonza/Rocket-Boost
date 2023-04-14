using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Variables
    private enum thrustKeyState { Off, Down, Up };
    private thrustKeyState ksSpace = thrustKeyState.Off;
    // private enum rotationKeyState { Off, Left, Right, Up };
    // private rotationKeyState ksADKeys = rotationKeyState.Off;

    private Rigidbody playerRb;
    private float mainThrustSpeed = 1000;
    private float rotationSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        // Change the state of the player key pressed down
        // and carry functionality over to FixedUpdate().
        if (Input.GetKey(KeyCode.Space))
        {
            ksSpace = thrustKeyState.Down;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            ksSpace = thrustKeyState.Up;
        }
    }

    private void FixedUpdate()
    {
        // These if statements are carried over from ProcessThrust()
        // function.
        if (ksSpace == thrustKeyState.Down)
        {
            // Holding "Jump"
            playerRb.AddRelativeForce(Vector3.up * mainThrustSpeed * Time.deltaTime, ForceMode.Acceleration);
        }
        else if (ksSpace == thrustKeyState.Up)
        {
            ksSpace = thrustKeyState.Off;
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
        }
    }

    void ApplyRotation(float rotationSpeedThisFrame)
    {
        playerRb.freezeRotation = true; // Freeze rotation so we can manually rotate.
        transform.Rotate(Vector3.forward * rotationSpeedThisFrame * Time.deltaTime);
        //playerRb.freezeRotation = false; // Unfreeze rotation so the physics system can take over.
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezePositionZ;
    }
}
