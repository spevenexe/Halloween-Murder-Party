using UnityEngine.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int gameOverIndex = 2, gameCompleteIndex = 3;

    public void LoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene((scene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings, LoadSceneMode.Single);
    }
    
    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene((scene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings, LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverIndex,LoadSceneMode.Single);
    }

    public void WinGame()
    {
        SceneManager.LoadScene(gameCompleteIndex,LoadSceneMode.Single);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
