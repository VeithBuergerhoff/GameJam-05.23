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

    private TextMeshProUGUI _textField;

    void Awake()
    {
        _textField = GetComponentInChildren<TextMeshProUGUI>();
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
        if (_overlayEntry is not null)
        {
            _overlayEntry._textField.text = _currentTask?.Name ?? string.Empty;
            var patiencePercentage = PatienceLeft / _currentTask?.Patience;
            _overlayEntry._timeSlider.value = patiencePercentage ?? 0;
        }

        if (PatienceLeft > 0)
        {
            PatienceLeft -= Time.deltaTime;
        }

        if (!HasTask)
        {
            _textField.text = ":)";
        }
        else if (HasPatienceLeft)
        {
            _textField.text = StatusMessage;
        }
        else
        {
            _textField.text = ">:(";
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
