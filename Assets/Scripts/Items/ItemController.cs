using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IInteractableItem
{
    [Serializable]
    public struct Item
    {
        public string Name;
    }

    [SerializeField]
    private Item _item;

    [SerializeField]
    private GameObject itemLabelPrefab;
    private GameObject itemLabelInstance;

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
        itemLabelInstance.GetComponent<LabelController>().SetText(_item.Name, _item.Name[0]);
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

    public string GetItemName()
    {
        return _item.Name;
    }

    public char GetCurrentHotkey()
    {
        return itemLabelInstance.GetComponent<LabelController>().GetHotkey();
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey = HotkeyHelper.findUniqueHotkey(_item.Name, usedHotkeys);
        itemLabelInstance.GetComponent<LabelController>().SetHotkey(hotkey);
        return hotkey;
    }

    public void ShowLabel(bool show)
    {
        itemLabelInstance.SetActive(show);
    }

    ~ItemController()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
