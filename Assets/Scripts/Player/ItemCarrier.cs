using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ItemCarrier : MonoBehaviour
{
    [SerializeField]
    private Transform _rotatingTransform;

    [SerializeField]
    private Transform _itemParent;

    [SerializeField]
    private float _itemDropoffDistance = 2;

    [SerializeField]
    private float _itemDropoffHeight = 1;

    [SerializeField]
    private string[] _pickupKeys;

    private ItemController _currentInteractableItem;

    private readonly Dictionary<string, ItemController> _items = new();
    private readonly ConcurrentQueue<ItemController> _colliderEnableQueue = new();


    void Awake()
    {

        if (_rotatingTransform is null)
        {
            Debug.LogError($"{nameof(_rotatingTransform)} must be set");
        }

        if (_itemParent is null)
        {
            Debug.LogError($"{nameof(_itemParent)} must be set");
        }
    }

    void Update()
    {
        foreach (var key in _pickupKeys)
        {
            if (Input.GetKeyDown(key))
            {
                PickUp(key);
            }

            if (Input.GetKeyUp(key))
            {
                Drop(key);
            }
        }
    }

    void FixedUpdate()
    {
        if (_colliderEnableQueue.TryDequeue(out var item))
        {
            var rigidbody = item.GetComponent<Rigidbody>();
            rigidbody.detectCollisions = true;
            rigidbody.isKinematic = false;

            var colliders = item.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (_currentInteractableItem is not null || !other.CompareTag("Interactable"))
        {
            return;
        }
        
        var item = other.GetComponentInParent<ItemController>();
        if (item is null)
        {
            return;
        }
        _currentInteractableItem = item;
    }

    void OnTriggerExit(Collider other)
    {
        _currentInteractableItem = null;
    }

    private void PickUp(string key)
    {
        var item = _currentInteractableItem;
        _currentInteractableItem = null;

        if (item is null)
        {
            return;
        }

        if (_items.ContainsKey(key))
        {
            Drop(key);
        }

        _items.Add(key, item);

        var colliders = item.GetComponentsInChildren<BoxCollider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        var rigidbody = item.GetComponent<Rigidbody>();
        rigidbody.detectCollisions = true;
        rigidbody.isKinematic = true;

        item.transform.SetParent(_itemParent);
        item.transform.localPosition = Vector3.zero;
    }

    private void Drop(string key)
    {
        if (_items.TryGetValue(key, out var item))
        {
            _items.Remove(key);
            item.transform.parent = null;
            item.transform.position = transform.position + _rotatingTransform.TransformDirection(Vector3.forward) * _itemDropoffDistance + new Vector3(0, _itemDropoffHeight, 0);
            item.transform.LookAt(transform, Vector3.up);

            _colliderEnableQueue.Enqueue(item);

            _currentInteractableItem = null;
        }
    }
}