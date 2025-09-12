using System;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] public bool isPaused;

    public static PauseGame instance;

    private void Awake()
    {
        instance = this;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f; 
        // Debug.Log("Game Paused");
    }

    public void Unpause()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f; 
        // Debug.Log("Game Unpaused");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}