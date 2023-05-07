using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechbubbleController : MonoBehaviour
{
    private bool _hasTask = false;
    public bool HasTask
    {
        get { return _hasTask; }
        set
        {
            if (_hasTask != value)
            {
                if (player is null)
                {
                    ShowIndicator(true);
                }
            }
            _hasTask = value;
        }
    }

    [SerializeField]
    private TextMeshProUGUI descriptionField;

    [SerializeField]
    private Slider indicatorSlider;

    [SerializeField]
    [Range(0, 10)]
    public float verticalPositionOffset = 2f;

    [SerializeField]
    public int padding = 24;

    private RectTransform _rectTransform;
    private CameraController _cameraController;

    private GameObject speechbubbleBackground;
    private GameObject taksIndicatorBackground;

    [SerializeField]
    private SensorController _playerSensor;
    private GameObject player;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cameraController = FindObjectOfType<CameraController>();
        speechbubbleBackground = transform.Find("background").gameObject;
        taksIndicatorBackground = transform.Find("taskIndikatorBackground").gameObject;
    }

    void Update()
    {
        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            _cameraController.mainCamera.transform.rotation
        );
    }

    public void SetText(string description, float patiencePercentage)
    {
        descriptionField.SetText(description);
        indicatorSlider.value = patiencePercentage;
        descriptionField.ForceMeshUpdate();
        var itemDescriptionBounds = descriptionField.GetRenderedValues();
        _rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            itemDescriptionBounds.y + padding
        );
    }

    private void ShowLabel(bool show)
    {
        speechbubbleBackground.SetActive(show);
    }

    private void ShowIndicator(bool show)
    {
        taksIndicatorBackground.SetActive(show);
    }

    void PlayerEnteredArea(object sender, Collider playerCollider)
    {
        if (player is null)
        {
            player = playerCollider.gameObject;
            if (_hasTask)
            {
                ShowLabel(true);
            }
            ShowIndicator(false);
        }
    }

    void PlayerExitedArea(object sender, Collider playerCollider)
    {
        if (player is not null && player == playerCollider.gameObject)
        {
            player = null;
            ShowLabel(false);
            if (_hasTask)
            {
                ShowIndicator(true);
            }
        }
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
