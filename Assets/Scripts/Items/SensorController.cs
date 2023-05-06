using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    private String watchedTag = "Player";
    
    public Action<bool> OnTagDetected;

    private bool m_isTagDetected = false;
    public bool isTagDetected
    {
        get { return m_isTagDetected; }
        private set
        {
            m_isTagDetected = value;
            OnTagDetected?.Invoke(value);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(watchedTag) && !isTagDetected)
        {
            isTagDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isTagDetected = false;
    }
}
