using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public bool active = false;
    public void Setup()
    {
        gameObject.SetActive(true);
        active = true;
        Time.timeScale = 0f;
    }

    
    public void RestartButton()
    {
        active = false;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {
        active = false;
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync(0);
    }
}
