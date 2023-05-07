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

    public char GetHotkey(int n = 0)
    {
        if (n < _item.Name.Length)
        {
            itemLabelInstance.GetComponent<LabelController>().SetHotkey(_item.Name.ToUpper()[n]);
            return _item.Name.ToUpper()[n];
        }
        else
        {
            throw new IndexOutOfRangeException(
                $"Itemname '${_item.Name}' does not have ${n} letters, use a smaller index"
            );
        }
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey;
        var uniqueCharsInName = _item.Name.ToUpper().Except(usedHotkeys);
        if (uniqueCharsInName.Any())
        {
            hotkey = uniqueCharsInName.First();
        }
        else
        {
            var uniqueCharsInAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Except(usedHotkeys);
            if (uniqueCharsInAlphabet.Any())
            {
                hotkey = uniqueCharsInAlphabet.ElementAt(
                    UnityEngine.Random.Range(0, uniqueCharsInAlphabet.Count() - 1)
                );
            }
            else
            {
                throw new IndexOutOfRangeException("There are no hotkeys left :(");
            }
        }

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
