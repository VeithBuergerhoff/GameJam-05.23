using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemController : MonoBehaviour, IInteractableItem
{
    [Serializable]
    public struct Item
    {
        public string Name;
    }

    [SerializeField]
    public Item item;

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
        itemLabelInstance.GetComponent<LabelController>().SetText(item.Name, item.Name[0]);
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

    #region IInteractableItem
    public string GetItemName()
    {
        return item.Name;
    }

    public char GetCurrentHotkey()
    {
        return itemLabelInstance.GetComponent<LabelController>().GetHotkey();
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey = HotkeyHelper.findUniqueHotkey(item.Name, usedHotkeys);
        itemLabelInstance.GetComponent<LabelController>().SetHotkey(hotkey);
        return hotkey;
    }

    public ItemController GetItemController() {
        return this;
    }
    #endregion IInteractableItem
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
