using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _mainMenuSceneIndex = 0;

    [SerializeField]
    private int _gameSceneIndex = 1;

    [SerializeField]
    private int _gameLostSceneIndex = 2;

    [SerializeField]
    private int _gameWonSceneIndex = 3;

    private object _sceneLoaderLock = new();
    private bool _isLoadingScene = false;

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

    public IEnumerator LoadMenu()
    {
        return LoadScene(_mainMenuSceneIndex);
    }

    public IEnumerator StartGame()
    {
        return LoadScene(_gameSceneIndex);
    }

    public IEnumerator InitiateGameOver()
    {
        return LoadScene(_gameLostSceneIndex);
    }

    public IEnumerator WinGame()
    {
        return LoadScene(_gameWonSceneIndex);
    }

    private IEnumerator LoadScene(int index)
    {
        if (_isLoadingScene)
        {
            yield break;
        }

        lock (_sceneLoaderLock)
        {
            _isLoadingScene = true;
            var asyncLoad = SceneManager.LoadSceneAsync(index);
            asyncLoad.completed += _ => _isLoadingScene = false;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
