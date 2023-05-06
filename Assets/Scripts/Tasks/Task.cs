using System;
using UnityEngine;

[Serializable]
public class Task
{
    public string Name;

    public int Patience;

    [Range(0, 1)]
    public float Frequency;
}