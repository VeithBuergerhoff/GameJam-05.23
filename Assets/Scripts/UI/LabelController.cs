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

    private RectTransform _rectTransform;
    private CameraController _cameraController;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        SetText(description, hotkey);
    }

    void Update()
    {
        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            _cameraController.transform.rotation
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
        _rectTransform.SetSizeWithCurrentAnchors(
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
