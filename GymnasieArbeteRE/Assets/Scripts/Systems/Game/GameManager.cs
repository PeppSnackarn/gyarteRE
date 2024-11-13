using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public gameState currentState;
    public static event Action<gameState> onGameStateChanged; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        UpdateGameState(gameState.MainMenuState);
    }

    public void UpdateGameState(gameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case gameState.MainMenuState:
                break;
            case gameState.PlayState:
                Time.timeScale = 1.0f;
                break;
            case gameState.PauseState:
                Time.timeScale = 0.0f;
                break;
        }
        onGameStateChanged.Invoke(newState);
    }

    public enum gameState
    {
        MainMenuState,
        PlayState,
        PauseState
    }
}
