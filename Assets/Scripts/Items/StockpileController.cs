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
    private GameObject labelPrefab;
    private GameObject labelInstance;
    
    [SerializeField]
    private Transform itemSpwanpoint;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private GameObject PlayerSensor;  
    private SensorController sensorController;    
    private GameObject player;
    

    void Awake()
    {
        sensorController = PlayerSensor.GetComponent<SensorController>();

        labelInstance = Instantiate(labelPrefab, transform);
        labelInstance.SetActive(false);
        labelInstance.GetComponent<LabelController>().SetText($"{itemName} nehmen", ' ');
    }

    void Update()
    {
        if (player is not null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ItemLifecycleManager.Instance.SpawnItem(
                    itemSpwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    itemSpwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    itemSpwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    itemSpwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    itemSpwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
            }
        }
    }

    public void ShowLabel(bool show)
    {
        labelInstance.SetActive(show);
    }

    void PlayerEnteredArea(Collider playerCollider)
    {
        if (player is null)
        {
            player = playerCollider.gameObject;
            ShowLabel(true);
        }
    }

    void PlayerExitedArea(Collider playerCollider)
    {
        if (player is not null && player == playerCollider.gameObject)
        {
            player = null;
            ShowLabel(false);
        }
    }

    void OnEnable()
    {
        sensorController.OnTagEnter += PlayerEnteredArea;
        sensorController.OnTagExit += PlayerExitedArea;
    }

    void OnDisable()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
