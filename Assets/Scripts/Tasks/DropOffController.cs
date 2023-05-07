using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DropOffController : MonoBehaviour
{
    public event EventHandler<ItemController> ItemEnter;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Interactable"))
        {
            return;
        }

        var item = other.GetComponentInParent<ItemController>();
        if (item is null)
        {
            return;
        }

        ItemEnter?.Invoke(this, item);
    }
}
