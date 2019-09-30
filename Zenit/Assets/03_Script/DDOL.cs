using UnityEngine;

public class DDOL : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("Don't destroy " + gameObject.name + " on load.");
    }
}
