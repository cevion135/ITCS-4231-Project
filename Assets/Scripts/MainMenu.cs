using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject creditsMenu;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);

    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);

    }

}