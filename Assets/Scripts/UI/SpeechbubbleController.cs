using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechbubbleController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionField;

    [SerializeField]
    [Range(0, 10)]
    public float verticalPositionOffset = 2f;

    [SerializeField]
    public int padding = 24;

    [SerializeField]
    private int preferredWidth = 256;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            CameraController.Instance.getMainCamera().transform.rotation
        );
    }

    public void SetText(string description)
    {
        descriptionField.SetText(description);
        descriptionField.ForceMeshUpdate();
        var itemDescriptionBounds = descriptionField.GetRenderedValues();
        if (description == ":)" || description == ">:(")
        {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                itemDescriptionBounds.x + padding
            );
        } else {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                preferredWidth
            );
        }
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            itemDescriptionBounds.y + padding
        );
    }
}
