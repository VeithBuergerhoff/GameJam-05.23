using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelController : MonoBehaviour
{
    [SerializeField]
    private RectTransform keycapIcon;

    [SerializeField]
    private TextMeshProUGUI itemDescription;

    [SerializeField]
    private TextMeshProUGUI iconText;

    [SerializeField]
    [Range(0, 10)]
    public float verticalPositionOffset = 2f;

    [SerializeField]
    private int padding = 32;

    [SerializeField]
    private string description = "Item Description";

    [SerializeField]
    private char hotkey = 'D';

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetText(description, hotkey);
    }

    void Update()
    {
        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            CameraController.Instance.getMainCamera().transform.rotation
        );
    }

    public void SetText(string description, char hotkey)
    {
        this.description = description;
        this.hotkey = hotkey;

        iconText.SetText(hotkey.ToString());
        itemDescription.SetText(description);
        itemDescription.ForceMeshUpdate();
        var itemDescriptionBounds = itemDescription.GetRenderedValues();
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            itemDescriptionBounds.x + padding + keycapIcon.rect.size.x * keycapIcon.localScale.x
        );
    }

    public void SetHotkey(char hotkey)
    {
        this.hotkey = hotkey;
        iconText.SetText(hotkey.ToString());
    }

    public char GetHotkey(){
        return hotkey;
    }
}
