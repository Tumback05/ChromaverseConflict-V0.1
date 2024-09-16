using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void QuitOut()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
