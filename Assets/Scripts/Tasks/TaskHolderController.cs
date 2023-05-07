using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskHolderController : MonoBehaviour
{
    [SerializeField]
    private DropOffController _dropOffController;

    private TaskOverlayEntry _overlayEntry;

    private Task _currentTask = null;

    public bool HasTask => _currentTask is not null;

    public bool HasPatienceLeft => PatienceLeft > 0;

    public float PatienceLeft { get; private set; }

    public float? MaxPatience => _currentTask?.Patience;

    private string StatusMessage => $"{_currentTask.Name}:{PatienceLeft:0}";

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
        if (PatienceLeft > 0)
        {
            PatienceLeft -= Time.deltaTime;
        }
        var patiencePercentage = (PatienceLeft / _currentTask?.Patience) ?? 0;
        
        if (_overlayEntry is not null)
        {
            _overlayEntry.textField.text = _currentTask?.Name ?? string.Empty;
            _overlayEntry.timeSlider.value = patiencePercentage;
        }        

        if (!HasTask)
        {
            _speechbubbleController.SetText(":)", patiencePercentage);
        }
        else if (HasPatienceLeft)
        {
            _speechbubbleController.HasTask = true;
            _speechbubbleController.SetText(
                _currentTask?.Name ?? string.Empty,
                patiencePercentage
            );
        }
        else
        {
            _speechbubbleController.SetText(">:(", patiencePercentage);
        }
    }

    public void QueueTask(Task task)
    {
        _currentTask = task;
        PatienceLeft = task.Patience;
    }

    public void CompleteTask()
    {
        _currentTask = null;
        Destroy(_overlayEntry.gameObject);
        _overlayEntry = null;
    }

    public void SetOverlayItem(TaskOverlayEntry taskOverlayEntry)
    {
        if (_overlayEntry is not null)
        {
            Destroy(_overlayEntry.gameObject);
            _overlayEntry = null;
        }

        _overlayEntry = taskOverlayEntry;
    }
}
