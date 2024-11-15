using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;
   private Player_IA InputAction;
   
   [Header("UI Objects")]
   [SerializeField] private GameObject mainMenuUI;
   [SerializeField] private GameObject playUI;
   [SerializeField] private GameObject pauseUI;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         GameManager.onGameStateChanged += UpdateCurrentUI;
      }
      else
      {
         Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
   }

   private void Start()
   {
      InputAction = GameManager.Instance.playerInput;
      InputAction.Player.Pause.performed += ctx => PauseGame();
   }

   private void OnDestroy()
   {
      GameManager.onGameStateChanged -= UpdateCurrentUI;
   }

   void UpdateCurrentUI(GameManager.gameState currentState)
   {
      mainMenuUI.SetActive(currentState == GameManager.gameState.MainMenuState);
      playUI.SetActive(currentState == GameManager.gameState.PlayState);
      pauseUI.SetActive(currentState == GameManager.gameState.PauseState);
   }

   void PauseGame()
   {
      if (GameManager.Instance.currentState == GameManager.gameState.PlayState)
      {
         GameManager.Instance.UpdateGameState(GameManager.gameState.PauseState);
      }
      else if (GameManager.Instance.currentState == GameManager.gameState.PauseState)
      {
         GameManager.Instance.UpdateGameState(GameManager.gameState.PlayState);
      }
   }
}
