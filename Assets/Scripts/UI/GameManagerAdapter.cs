using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAdapter : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMenu();
    }
}
