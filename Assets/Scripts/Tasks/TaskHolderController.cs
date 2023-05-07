using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskHolderController : MonoBehaviour
{
    [SerializeField]
    private DropOffController _dropOffController;

    private Task _currentTask = null;

    public bool HasTask => _currentTask is not null;

    public bool HasPatienceLeft => _patienceLeft > 0;

    private float _patienceLeft;

    private string StatusMessage => $"{_currentTask.Name}:{_patienceLeft:0}";

    private SpeechbubbleController _speechbubbleController;

    void Awake()
    {
        _speechbubbleController = GetComponentInChildren<SpeechbubbleController>();
    }

    void OnEnable()
    {
        _dropOffController.ItemEnter += OnItemEnteredDropOffZone;
    }

    void OnDisable()
    {
        _dropOffController.ItemEnter -= OnItemEnteredDropOffZone;
    }

    private void OnItemEnteredDropOffZone(object sender, ItemController e)
    {
        if (e.item.Name == _currentTask?.WantedItemName)
        {
            CompleteTask();
            ItemLifecycleManager.Instance.RemoveItem(e);
        }
    }

    void Update()
    {
        if (_patienceLeft > 0)
        {
            _patienceLeft -= Time.deltaTime;
        }

        if (!HasTask)
        {
            _speechbubbleController.SetText(":)");
        }
        else if (HasPatienceLeft)
        {
            _speechbubbleController.SetText(StatusMessage);
        }
        else
        {
            _speechbubbleController.SetText(">:(");
        }
    }

    public void QueueTask(Task task)
    {
        _currentTask = task;
        _patienceLeft = task.Patience;
    }

    public void CompleteTask()
    {
        _currentTask = null;
    }
}
