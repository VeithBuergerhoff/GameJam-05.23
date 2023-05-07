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
            throw new Exception($"{nameof(ItemLifecycleManager)} is a singleton! There can only ever be one instance");
        }

        Instance = this;
    }

    public ItemController SpawnItem()
    {
        return SpawnItem(Vector3.zero, Quaternion.identity);
    }

    public ItemController SpawnItem(Vector3 position)
    {
        return SpawnItem(position, Quaternion.identity);
    }

    public ItemController SpawnItem(Vector3 position, Quaternion rotation)
    {
        if (_itemPool.Any())
        {
            var item = _itemPool.Single();
            item.transform.SetParent(null);
            item.gameObject.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
            return item;
        }

        return Instantiate(_itemPrefab, position, rotation).GetComponent<ItemController>();
    }

    public void RemoveItem(ItemController item)
    {
        item.transform.SetParent(this.transform);
        item.gameObject.SetActive(false);
        _itemPool.Add(item);
    }
}
