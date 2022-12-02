using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SavedGame savedGame;

    public void ContinueGame()
    {
        SceneManager.LoadScene(3);
    }

   public void StartNewGame()
    {
        // SaveManager.Instance.Delete(savedGame);
        SceneManager.LoadScene(3);
        //FadeInOut.sceneEnd = true;
        // load scene the next by index
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        FadeInOut.sceneEnd = true;
        Application.Quit();
    }
}
