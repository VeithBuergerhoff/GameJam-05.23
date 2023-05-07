using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    private String _watchedTag = "Player";
    
    public Action<bool> OnTagDetected;

    public Action<Collider> OnTagEnter;
    public Action<Collider> OnTagExit;

    private bool _isTagDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_watchedTag) && !_isTagDetected)
        {
            _isTagDetected = true;
            OnTagEnter?.Invoke(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_watchedTag) && _isTagDetected)
        {
            _isTagDetected = false;
            OnTagExit?.Invoke(other);
        }
    }
}
