using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    public bool active = false;

    public void Setup()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void RestartButton()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        SceneManager.LoadScene(sceneName);
        gameObject.SetActive(false);
    }


    public void NextLevelButton()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync(0);
    }

    public void Credits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }
}
