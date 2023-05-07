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
        public char PreferredHotkey;
    }

    [SerializeField]
    public Item item;

    [SerializeField]
    private GameObject itemLabelPrefab;
    private GameObject itemLabelInstance;
    private LabelController itemLabelController;

    [SerializeField]
    private SensorController _playerSensor;

    void Start()
    {
        itemLabelInstance = Instantiate(itemLabelPrefab, transform);
        itemLabelInstance.SetActive(false);

        itemLabelController = itemLabelInstance.GetComponent<LabelController>();
        itemLabelController.verticalPositionOffset = 1f;
        itemLabelController.SetText(item.Name, item.PreferredHotkey);
    }

    void PlayerEnteredArea(object sender, Collider playerCollider)
    {           
        playerCollider.gameObject.GetComponent<ItemCarrier>().registerInteractableItem(this);
        ShowLabel(true);
    }

    void PlayerExitedArea(object sender, Collider playerCollider)
    {
        playerCollider.gameObject.GetComponent<ItemCarrier>().deregisterInteractableItem(this);
        ShowLabel(false);
    }

    public void SetText(Item itemInfo){
        item = itemInfo;
        itemLabelController.SetText(itemInfo.Name, itemInfo.PreferredHotkey);
    }

    #region IInteractableItem
    public string GetItemName()
    {
        return item.Name;
    }

    public char GetPreferredHotkey()
    {
        return item.PreferredHotkey;
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey = HotkeyHelper.findUniqueHotkey(item.Name, usedHotkeys);
        item.PreferredHotkey = hotkey;
        itemLabelController.SetHotkey(hotkey);
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
