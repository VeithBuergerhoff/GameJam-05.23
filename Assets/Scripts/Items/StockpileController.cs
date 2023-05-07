using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockpileController : MonoBehaviour, IInteractableItem
{
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private GameObject itemLabelPrefab;
    private GameObject itemLabelInstance;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private GameObject PlayerSensor;
    private SensorController sensorController;

    void Awake()
    {
        sensorController = PlayerSensor.GetComponent<SensorController>();
        sensorController.OnTagEnter += PlayerEnteredArea;
        sensorController.OnTagExit += PlayerExitedArea;

        itemLabelInstance = Instantiate(itemLabelPrefab, transform);
        itemLabelInstance.SetActive(false);
        itemLabelInstance.GetComponent<LabelController>().SetText(itemName, itemName[0]);
    }

    #region IInteractableItem
    public string GetItemName()
    {
        return itemName;
    }

    public char GetCurrentHotkey()
    {
        return itemLabelInstance.GetComponent<LabelController>().GetHotkey();
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey = HotkeyHelper.findUniqueHotkey(itemName, usedHotkeys);
        itemLabelInstance.GetComponent<LabelController>().SetHotkey(hotkey);
        return hotkey;
    }

    public ItemController GetItemController() {
        throw new NotImplementedException("todo: spwan item");
    }
    #endregion IInteractableItem

    public void ShowLabel(bool show)
    {
        itemLabelInstance.SetActive(show);
    }

    void PlayerEnteredArea(Collider playerCollider)
    {
        playerCollider.gameObject.GetComponent<ItemCarrier>().registerInteractableItem(this);
        ShowLabel(true);
    }

    void PlayerExitedArea(Collider playerCollider)
    {
        playerCollider.gameObject.GetComponent<ItemCarrier>().deregisterInteractableItem(this);
        ShowLabel(false);
    }

    ~StockpileController()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
