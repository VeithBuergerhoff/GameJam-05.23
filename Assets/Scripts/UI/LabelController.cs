using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LabelController : MonoBehaviour
{
    [SerializeField]
    private RectTransform background;
    
    [SerializeField]
    private RectTransform keycapIcon;
    
    [SerializeField]
    private TextMeshProUGUI itemDescription;

    [SerializeField]
    private TextMeshProUGUI iconText;

    [SerializeField]
    private int padding = 32;

    [SerializeField]
    private string Description = "Item Description";

    [SerializeField]
    private char hotkey = 'D';

    private void Start()
    {        
        SetText(Description, hotkey);
    }

    private void SetText(string text, char hotkey)
    {
        itemDescription.SetText(text);
        itemDescription.ForceMeshUpdate();
        iconText.SetText(hotkey.ToString());
        var itemDescriptionBounds = itemDescription.GetRenderedValues();
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemDescriptionBounds.x + padding + keycapIcon.rect.size.x * keycapIcon.localScale.x );
    }
}
