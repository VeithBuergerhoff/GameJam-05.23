using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [SerializeField]
    private GameObject _taskOverlay;

    [SerializeField]
    private GameObject _overlayTaskTemplate;

    [SerializeField]
    private int _interval = 30;

    [SerializeField]
    private int _batchSize = 2;

    [SerializeField]
    private Task[] _taskPool;

    [SerializeField]
    private TaskHolderController[] _taskHolder;

    private float _countdown = 3;
    private IList<Task> _randomTasks;
    private int _taskOffset;

    void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            throw new Exception($"{nameof(TaskManager)} is a singleton! There can only ever be one instance");
        }

        Instance = this;
    }

    void Update()
    {
        if (_countdown <= 0)
        {
            _countdown = _interval;

            QueueTasks();
        }
        else
        {
            _countdown -= Time.deltaTime;
        }

        if (_taskHolder.Any(x => x.HasTask && !x.HasPatienceLeft))
        {
            StartCoroutine(GameManager.Instance.InitiateGameOver());
        }
    }

    private TaskOverlayEntry AddOverlayItem()
    {
        var newOverlayItem = Instantiate(_overlayTaskTemplate);
        newOverlayItem.transform.SetParent(_taskOverlay.transform);
        
        return newOverlayItem.GetComponent<TaskOverlayEntry>();
    }

    private void QueueTasks()
    {
        Debug.Log("Time to queue some tasks");

        CreateTasks();

        var availableTaskHolders = _taskHolder.Where(x => !x.HasTask).ToList().Shuffle().Take(_batchSize);

        Debug.Log($"Queuing {availableTaskHolders.Count()} tasks");

        foreach (var taskHolder in availableTaskHolders)
        {
            taskHolder.QueueTask(_randomTasks[_taskOffset++]);
            taskHolder.SetOverlayItem(AddOverlayItem());
        }
    }

    private void CreateTasks()
    {
        if (_randomTasks is null || _randomTasks.Skip(_taskOffset).Count() < _batchSize)
        {
            Debug.Log("Creating new tasks...");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            _randomTasks = GenerateRandomTasks();
            _taskOffset = 0;
            watch.Stop();

            Debug.Log($"It took {watch.Elapsed} to generate random tasks");
        }
        else
        {
            Debug.Log($"Currently at task offset {_taskOffset} with {_randomTasks.Count} total tasks");
        }
    }

    private IList<Task> GenerateRandomTasks()
    {
        var enumerableTasks = new List<Task>();

        foreach (var task in _taskPool)
        {
            for (int i = 0; i < task.Frequency * 100; i++)
            {
                enumerableTasks.Add(task);
            }
        }

        return enumerableTasks.Shuffle();
    }
}
