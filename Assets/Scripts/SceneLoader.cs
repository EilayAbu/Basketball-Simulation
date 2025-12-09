using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // ???? ????? ??? ??
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ???? ?????? ???? ??????
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextIndex);
    }

    // ???? ?????? ?????
    public void LoadPreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int prevIndex = currentIndex - 1;
        if (prevIndex < 0)
            prevIndex = SceneManager.sceneCountInBuildSettings - 1;

        SceneManager.LoadScene(prevIndex);
    }
}
