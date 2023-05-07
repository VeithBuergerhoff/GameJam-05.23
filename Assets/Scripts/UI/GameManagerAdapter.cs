using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAdapter : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(GameManager.Instance.StartGame());
    }

    public void LoadMainMenu()
    {
        StartCoroutine(GameManager.Instance.LoadMenu());
    }
}
