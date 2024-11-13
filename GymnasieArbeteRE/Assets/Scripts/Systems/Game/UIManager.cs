using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;
   
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
         PlayerData.Instance.playerMovement.InputAction.Player.Pause.performed += ctx => PauseGame(); // need to find player;
         // Player spawns in second scene, rework this or bind an event
      }
      else
      {
         Destroy(this);
      }
      DontDestroyOnLoad(Instance);
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
   }
}
