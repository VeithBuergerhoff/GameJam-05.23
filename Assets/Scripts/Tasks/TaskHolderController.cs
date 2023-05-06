using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskHolderController : MonoBehaviour
{
    private Task _currentTask = null;

    public bool HasTask => _currentTask is not null;

    public bool HasPatienceLeft => _patienceLeft > 0;

    private float _patienceLeft;

    private string StatusMessage => $"{_currentTask.Name}:{_patienceLeft:0}";

    private TextMeshProUGUI _textField;

    void Awake()
    {
        _textField = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_patienceLeft > 0)
        {
            _patienceLeft -= Time.deltaTime;
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
        _patienceLeft = task.Patience;
    }

    public void CompleteTask()
    {
        _currentTask = null;
    }
}
