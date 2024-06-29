using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static GameObject instance;
    private void Start()
    {
        instance = gameObject;
        DontDestroyOnLoad(this);
    }
}
