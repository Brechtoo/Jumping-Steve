using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public MainMenu mainMenu;
    public MainMenu Level;
    public void PlayGame()
    {
        mainMenu.gameObject.SetActive(false);
        Level.gameObject.SetActive(true);
        
    }

    public void Back()
    {
        Level.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void Level1()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Level2()
    {
        SceneManager.LoadSceneAsync(4);
    }
}



