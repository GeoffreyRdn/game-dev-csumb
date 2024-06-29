using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    #region Events

    public delegate void PlayAgain();
    public static event PlayAgain Restart;

    #endregion
    
    public static bool hasLoose = false;

    private void Start()
    {
        hasLoose = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart?.Invoke();
            hasLoose = false;
        }
    }
}
