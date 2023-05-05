using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Variables
    int currentSceneIndex;

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with Launch Pad");
                break;
            case "Finish":
                LoadNextLevel();
                Debug.Log("Collided with Landing Pad");
                break;
            case "Fuel":
                Debug.Log("Collided with Fuel Cell");
                break;
            default:
                ReloadLevel();
                Debug.Log("Sorry, you blew up!");
                break;
        }
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
