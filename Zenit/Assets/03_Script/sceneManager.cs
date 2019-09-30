using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void OnSceneChange(int n)
    {
        if (n <= SceneManager.sceneCountInBuildSettings - 1)
        SceneManager.LoadScene(n);
    }
}
