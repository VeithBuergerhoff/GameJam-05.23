using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechbubbleController : MonoBehaviour
{
    private bool _hasTask = false;
    public bool HasTask
    {
        get { return _hasTask; }
        set { 
            if(_hasTask != value){
                if(player is null){
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

    [SerializeField]
    private int preferredWidth = 256;

    private RectTransform rectTransform;
    private GameObject speechbubbleBackground;
    private GameObject taksIndicatorBackground;

    [SerializeField]
    private GameObject playerSensor;
    private SensorController sensorController;
    private GameObject player;

    void Awake()
    {
        sensorController = playerSensor.GetComponent<SensorController>();
        rectTransform = GetComponent<RectTransform>();
        speechbubbleBackground = transform.Find("background").gameObject;
        taksIndicatorBackground = transform.Find("taskIndikatorBackground").gameObject;
    }

    void Update()
    {
        transform.SetPositionAndRotation(
            transform.parent.position + Vector3.up * verticalPositionOffset,
            CameraController.Instance.getMainCamera().transform.rotation
        );
    }

    public void SetText(string description, float patiencePercentage)
    {
        descriptionField.SetText(description);
        indicatorSlider.value = patiencePercentage;
        descriptionField.ForceMeshUpdate();
        var itemDescriptionBounds = descriptionField.GetRenderedValues();
        if (description == ":)" || description == ">:(")
        {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                itemDescriptionBounds.x + padding
            );
        }
        else
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth);
        }
        rectTransform.SetSizeWithCurrentAnchors(
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

    void PlayerEnteredArea(Collider playerCollider)
    {
        if (player is null)
        {
            player = playerCollider.gameObject;
            ShowLabel(true);
            ShowIndicator(false);
        }
    }

    void PlayerExitedArea(Collider playerCollider)
    {
        if (player is not null && player == playerCollider.gameObject)
        {
            player = null;
            ShowLabel(false);
            ShowIndicator(true);
        }
    }

    void OnEnable()
    {
        sensorController.OnTagEnter += PlayerEnteredArea;
        sensorController.OnTagExit += PlayerExitedArea;
    }

    void OnDisable()
    {
        sensorController.OnTagEnter -= PlayerEnteredArea;
        sensorController.OnTagExit -= PlayerExitedArea;
    }
}
