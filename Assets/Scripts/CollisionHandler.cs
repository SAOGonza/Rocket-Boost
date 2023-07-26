using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Variables
    private int currentSceneIndex;
    private float levelLoadDelay = 2f;
    [HideInInspector] public bool doorIsOpen;

    // Class variables
    private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip openDoorSound;

    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    // State variables
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        // Load next level.
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        // Disable all collision.
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // Toggle collision.
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with Launch Pad");
                break;
            case "Finish":
                StartNextLevelSequence();
                Debug.Log("Collided with Landing Pad");
                break;
            case "Fuel":
                Debug.Log("Collided with Fuel Cell");
                break;
            case "Pressure Plate":
                OpenDoor();
                Debug.Log("Collided with Pressure Plate.");
                break;
            default:
                StartCrashSequence();
                Debug.Log("Sorry, you blew up!");
                break;
        }
    }

    private void StartNextLevelSequence()
    {
        // Stop rocket boost SFX on touchdown.
        isTransitioning = true;
        audioSource.Stop();

        // Play SFX upon touchdown.
        audioSource.PlayOneShot(successSound);
        // Play particle effect upon touchdown.
        successParticles.Play();

        // Stop player control for a second after landing.
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        // Stop rocket boost SFX on crash.
        isTransitioning = true;
        audioSource.Stop();

        // Play SFX upon crash.
        audioSource.PlayOneShot(crashSound);
        // Play particle effect upon crash.
        crashParticles.Play();

        // Stop player control for a second after crashing.
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void OpenDoor()
    {
        Debug.Log("Open Sesame!");
        doorIsOpen = true;
        audioSource.PlayOneShot(openDoorSound);
    }

    private void ReloadLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
