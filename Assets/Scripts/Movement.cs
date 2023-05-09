using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Variables
    private enum thrustKeyState { Off, Down, Up };
    private thrustKeyState ksSpace = thrustKeyState.Off;

    private Rigidbody playerRb;
    private float mainThrustSpeed = 1000;
    private float rotationSpeed = 100;

    // Class Variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem leftThrusterParticles;
    [SerializeField] private ParticleSystem rightThrusterParticles;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void FixedUpdate()
    {
        // These if statements are carried over from ProcessThrust()
        // function.
        if (ksSpace == thrustKeyState.Down)
        {
            // Holding "Jump"
            StartThrusting();
        }
        else if (ksSpace == thrustKeyState.Up)
        {
            // No longer holding "Jump".
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        // Holding "Jump".
        playerRb.AddRelativeForce(Vector3.up * mainThrustSpeed * Time.deltaTime, ForceMode.Acceleration);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void StopThrusting()
    {
        // No longer holding "Jump".
        ksSpace = thrustKeyState.Off;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        mainEngineParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);

        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);

        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    void ApplyRotation(float rotationSpeedThisFrame)
    {
        playerRb.freezeRotation = true; // Freeze rotation so we can manually rotate.
        transform.Rotate(Vector3.forward * rotationSpeedThisFrame * Time.deltaTime);
        //playerRb.freezeRotation = false; // Unfreeze rotation so the physics system can take over.
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezePositionZ;
    }


    private void OnDisable()
    {
        mainEngineParticles.Stop();
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }
}
