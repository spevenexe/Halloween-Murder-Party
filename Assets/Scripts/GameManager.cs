using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : Singleton<GameManager>
{
    public void LoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene((scene.buildIndex+1) % SceneManager.sceneCountInBuildSettings, LoadSceneMode.Single);
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
