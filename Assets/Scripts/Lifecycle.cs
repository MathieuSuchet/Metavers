using UnityEngine;
using UnityEngine.SceneManagement;

public class Lifecycle : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void Replay()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
