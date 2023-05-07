using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private RectTransform _rectTransform;
    
    private CameraController _cameraController;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {

        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            _cameraController.mainCamera.transform.rotation
        );
    }

    public void SetText(string description)
    {
        descriptionField.SetText(description);
        descriptionField.ForceMeshUpdate();
        var itemDescriptionBounds = descriptionField.GetRenderedValues();
        if (description == ":)" || description == ">:(")
        {
            _rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                itemDescriptionBounds.x + padding
            );
        } else {
            _rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                preferredWidth
            );
        }
        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            itemDescriptionBounds.y + padding
        );
    }
}
