using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockpileController : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private int _itemSpawnCount = 5;

    [SerializeField]
    private LabelController _label;

    [SerializeField]
    private string _itemName;

    [SerializeField]
    private SensorController _playerSensor;

    [SerializeField]
    private Transform _spawnpoint;

    private GameObject player;


    void Awake()
    {
        _label.SetText($"{_itemName} nehmen", ' ');
    }

    void Update()
    {
        if (player is not null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < _itemSpawnCount; i++)
                {
                    ItemLifecycleManager.Instance.SpawnItem(_spawnpoint.position,
                                                            new ItemController.Item() { Name = _itemName, PreferredHotkey = _itemName[0] });
                }
            }
        }
    }

    public void ShowLabel(bool show)
    {
        _label.gameObject.SetActive(show);
    }

    void PlayerEnteredArea(object sender, Collider playerCollider)
    {
        if (player is null)
        {
            player = playerCollider.gameObject;
            ShowLabel(true);
        }
    }

    void PlayerExitedArea(object sender, Collider playerCollider)
    {
        if (player is not null && player == playerCollider.gameObject)
        {
            player = null;
            ShowLabel(false);
        }
    }

    void OnEnable()
    {
        _playerSensor.TagEntered += PlayerEnteredArea;
        _playerSensor.TagExited += PlayerExitedArea;
    }

    void OnDisable()
    {
        _playerSensor.TagEntered -= PlayerEnteredArea;
        _playerSensor.TagExited -= PlayerExitedArea;
    }
}
