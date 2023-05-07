using System;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    private string _watchedTag = "Player";
    
    public event EventHandler<Collider> TagEntered;
    public event EventHandler<Collider> TagExited;

    private bool _isTagDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_watchedTag) && !_isTagDetected)
        {
            _isTagDetected = true;
            TagEntered?.Invoke(this, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_watchedTag) && _isTagDetected)
        {
            _isTagDetected = false;
            TagExited?.Invoke(this, other);
        }
    }
}
