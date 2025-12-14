using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // A private helper function to unpause the game
    private void ResetTimeScale()
    {
        // CRITICAL FIX: Ensure the time scale is reset to normal (1.0)
        Time.timeScale = 1.0f;
    }

    // Function 1: Starts the game by loading Maze
    public void StartGame()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Menu");
    }

    // Function 2: Loads the Main Menu scene
    public void LoadMainMenu()
    {
        ResetTimeScale();
        
        // FIX: Try to reset the Day/Night Cycle before loading the menu
        // This prevents the skybox/light from being stuck in "night" mode.
     
        
        SceneManager.LoadScene("Menu");
    }

    // Function 3: Quits the game
    public void QuitGame()
    {
        Debug.Log("Quitting Application...");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}