using UnityEngine; 
using UnityEngine.SceneManagement; 

public class NextLevel : MonoBehaviour 
{
    public void LoadNextLevel() 
    { 
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // Gets the next scene's build index
        SceneManager.LoadScene(nextSceneIndex); // Loads the next level
    } 

    public void LoadMainMenu() 
    { 
        SceneManager.LoadScene("MainMenu"); 
    } 
} 