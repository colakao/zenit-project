using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevel : MonoBehaviour
{
    // Update is called once per frame
    public void ReloadScene()
    {
        SceneManager.LoadScene(this.gameObject.scene.name);
        StarGenerator.mapGenerated = false;
    }

    public void SelectDifficulty(int n)
    {
        if (n <= 2)
        Difficulty.SetDifficulty(n);
    }
}
