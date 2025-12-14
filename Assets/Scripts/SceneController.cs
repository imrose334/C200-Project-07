using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneController : MonoBehaviour
{
    // Call this from Start button
    public void StartGame()
    {
        // Replace "GameScene" with the name of your main gameplay scene
        SceneManager.LoadScene("GameScene");
    }

    // Call this from Exit button
    public void ExitGame()
    {
        // Works in build
        Application.Quit();

        // Stops play mode in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Call this from Restart button on ending scene
    public void RestartGame()
    {
        // Reload the opening scene
        SceneManager.LoadScene("OpeningScene");
    }

    // Optional: Load a specific scene by name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Load scene by build index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
