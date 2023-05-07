using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<char, IInteractableItem> _interactableItems = new();
    private Dictionary<char, KeyCode> _pickupKeyCache = new();
    private int keyDelta = 32;

    private readonly Dictionary<char, ItemController> _items = new();
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
        foreach (var key in _interactableItems.Keys)
        {
            KeyCode keyCode;
            if (_pickupKeyCache.TryGetValue(key, out keyCode))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    PickUp(key);
                }

                if (Input.GetKeyUp(keyCode))
                {
                    Drop(key);
                }
            }
            else
            {
                TryAddKeyCodeToCache(key);
            }
        }
    }

    private bool TryAddKeyCodeToCache(char key)
    {
        KeyCode keyCode;
        int alphaValue = 0;
        // mapping to KeyCode requires lowercase Chars
        if (65 <= key && key <= 90)
        {
            alphaValue = key + keyDelta;
        }
        else if (97 <= key && key <= 122)
        {
            alphaValue = key;
        }
        if (alphaValue != 0)
        {
            keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
            _pickupKeyCache.Add(key, keyCode);
            return true;
        }
        keyCode = KeyCode.None;
        return false;
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

    private void PickUp(char key)
    {
        ItemController item = _interactableItems[key].GetItemController();

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

        int stackPosition = _items.Count - 1;

        item.ShowLabel(false);

        item.transform.SetParent(_itemParent);
        item.transform.localPosition = Vector3.up * stackPosition;
    }

    private void Drop(char key)
    {
        if (_items.TryGetValue(key, out var item))
        {
            _items.Remove(key);
            item.transform.parent = null;
            item.transform.position =
                transform.position
                + _rotatingTransform.TransformDirection(Vector3.forward) * _itemDropoffDistance
                + new Vector3(0, _itemDropoffHeight, 0);
            item.transform.LookAt(transform, Vector3.up);

            _colliderEnableQueue.Enqueue(item);
        }

        for (int i = 0; i < _items.Count(); i++)
        {
            _items.Values.ElementAt(i).transform.localPosition = Vector3.up * i;
        }
    }

    public void registerInteractableItem(IInteractableItem item)
    {
        if (!_interactableItems.Keys.Contains(item.GetPreferredHotkey()))
        {
            _interactableItems.Add(item.GetPreferredHotkey(), item);
        }
        else
        {
            _interactableItems.Add(item.GetUniqueHotkey(_interactableItems.Keys), item);
        }
    }

    public void deregisterInteractableItem(IInteractableItem item)
    {
        _interactableItems.Remove(item.GetPreferredHotkey());
    }
}
