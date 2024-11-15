using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public gameState currentState;
    private Player_IA InputAction;
    private bool bHasInput;

    #region Properties
    public Player_IA playerInput => InputAction;
    #endregion
    public static event Action<gameState> onGameStateChanged; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateGameState(gameState.MainMenuState);
        InputAction = new Player_IA();
        InputAction.Enable();
    }

    public void UpdateGameState(gameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case gameState.MainMenuState:
                SceneManager.LoadSceneAsync(0);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1.0f;
                break;
            case gameState.PlayState:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
                break;
            case gameState.PauseState:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
