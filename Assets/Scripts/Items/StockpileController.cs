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

    public char GetHotkey(int n = 0)
    {
        if (n < itemName.Length)
        {
            itemLabelInstance.GetComponent<LabelController>().SetHotkey(itemName.ToUpper()[n]);
            return itemName[n];
        } else {
            throw new IndexOutOfRangeException($"Itemname '${itemName}' does not have ${n} letters, use a smaller index");
        }
    }

    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys)
    {
        char hotkey;
        var uniqueCharsInName = itemName.ToUpper().Except(usedHotkeys);
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

    void PlayerEnteredArea(Collider playerCollider)
    {
        playerCollider.gameObject
            .GetComponent<ItemCarrier>()
            .registerInteractableItem(this);
        ShowLabel(true);
    }

    void PlayerExitedArea(Collider playerCollider)
    {
        playerCollider.gameObject
            .GetComponent<ItemCarrier>()
            .deregisterInteractableItem(this);
        ShowLabel(false);
    }

    ~StockpileController()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
