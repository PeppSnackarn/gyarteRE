using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.gameState.PlayState);
        SceneManager.LoadSceneAsync(1);
    }
}
