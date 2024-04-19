using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevels()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadSceneAsync(3);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }   
}
