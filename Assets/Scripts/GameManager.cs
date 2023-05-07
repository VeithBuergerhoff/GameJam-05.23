using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _gameSceneIndex = 1;

    public static GameManager Instance { get; set; }

    void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    public void StartGame()
    {
        SceneManager.LoadSceneAsync(_gameSceneIndex);
    }
}
