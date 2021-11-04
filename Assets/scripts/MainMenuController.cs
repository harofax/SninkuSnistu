using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private const int PlaySceneIndex = 1;
    
    public void OnStartButtonPress()
    {
        SceneManager.LoadScene(PlaySceneIndex);
    }

    public void OnEndButtonPress()
    {
        Application.Quit();
    }
}
