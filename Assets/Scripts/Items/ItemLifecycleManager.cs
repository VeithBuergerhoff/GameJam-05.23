using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLifecycleManager : MonoBehaviour
{
    public static ItemLifecycleManager Instance { get; private set; }

    [SerializeField]
    private GameObject _itemPrefab;

    private List<ItemController> _itemPool = new();

    void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
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
        if (_itemPool.Any(item => !item.isActiveAndEnabled))
        {
            var item = _itemPool.First();
            item.transform.SetParent(null);
            item.gameObject.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.name = $"Item: {initialValues.Name}";
            item.SetText(initialValues);
            item.gameObject.SetActive(true);
            return item;
        }

        var spawned = Instantiate(_itemPrefab, position, rotation);

        spawned.name = $"Item: {initialValues.Name}";
        spawned.SetActive(true);

        var controller = spawned.GetComponent<ItemController>();
        controller.item.Name = initialValues.Name;
        controller.item.PreferredHotkey = initialValues.PreferredHotkey;
        return controller;
    }

    public void RemoveItem(ItemController item)
    {
        item.transform.SetParent(this.transform);
        item.gameObject.SetActive(false);
        _itemPool.Add(item);
    }
}
