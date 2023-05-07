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
    private string itemName;

    [SerializeField]
    private GameObject PlayerSensor;
    [SerializeField]
    private Transform spwanpoint;
    
    private SensorController sensorController;
    private GameObject player;
    

    void Awake()
    {
        sensorController = PlayerSensor.GetComponent<SensorController>();
        sensorController.OnTagEnter += PlayerEnteredArea;
        sensorController.OnTagExit += PlayerExitedArea;

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
                    spwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    spwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    spwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    spwanpoint.position,                
                    new ItemController.Item() { Name = itemName, PreferredHotkey = itemName[0] }
                );
                ItemLifecycleManager.Instance.SpawnItem(
                    spwanpoint.position,                
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

    ~StockpileController()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
