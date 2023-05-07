using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLifecycleManager : MonoBehaviour
{
    public static ItemLifecycleManager Instance { get; private set; }

    [SerializeField]
    private GameObject _itemPrefab;

    private GameObject _editableInstantiator;
    private ItemController _editableItemController;

    private List<ItemController> _itemPool = new();

    void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            throw new Exception($"{nameof(ItemLifecycleManager)} is a singleton! There can only ever be one instance");
        }

        Instance = this;

        _editableInstantiator = Instantiate(_itemPrefab);
        _editableItemController = _editableInstantiator.GetComponent<ItemController>();
        _editableInstantiator.SetActive(false);
    }

    public ItemController SpawnItem(ItemController.Item initialValues)
    {
        return SpawnItem(Vector3.zero, Quaternion.identity, initialValues);
    }

    public ItemController SpawnItem(Vector3 position, ItemController.Item initialValues)
    {
        return SpawnItem(position, Quaternion.identity, initialValues);
    }

    public ItemController SpawnItem(Vector3 position, Quaternion rotation, ItemController.Item initialValues)
    {
        if (_itemPool.Any())
        {
            var item = _itemPool.Single();
            item.transform.SetParent(null);
            item.gameObject.transform.SetPositionAndRotation(position, rotation);
            item.transform.parent.name = $"Item: {initialValues.Name}";
            item.SetText( initialValues );
            item.gameObject.SetActive(true);
            return item;
        }

        _editableItemController.item.Name = initialValues.Name;
        _editableItemController.item.PreferredHotkey = initialValues.PreferredHotkey;
        var spawned = Instantiate(_editableInstantiator, position, rotation);
        spawned.name = $"Item: {initialValues.Name}";
        spawned.SetActive(true);
        return spawned.GetComponent<ItemController>();
    }

    public void RemoveItem(ItemController item)
    {
        item.transform.SetParent(this.transform);
        item.gameObject.SetActive(false);
        _itemPool.Add(item);
    }
}
