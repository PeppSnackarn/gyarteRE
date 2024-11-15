using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   public void MainMenu()
   {
      GameManager.Instance.UpdateGameState(GameManager.gameState.MainMenuState);
   }
   public void Continue()
   {
      GameManager.Instance.UpdateGameState(GameManager.gameState.PlayState);
   }
}
