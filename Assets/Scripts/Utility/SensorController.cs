using System;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    private string _watchedTag = "Player";
    
    public event EventHandler<Collider> TagEntered;
    public event EventHandler<Collider> TagExited;

    public bool isEntryInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_watchedTag) && !isEntryInZone)
        {
            isEntryInZone = true;
            TagEntered?.Invoke(this, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_watchedTag) && isEntryInZone)
        {
            isEntryInZone = false;
            TagExited?.Invoke(this, other);
        }
    }
}
