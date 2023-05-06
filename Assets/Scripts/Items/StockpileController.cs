using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileController : MonoBehaviour
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

    void Awake(){
        sensorController = PlayerSensor.GetComponent<SensorController>();
        sensorController.OnTagDetected += ShowLabel;

        itemLabelInstance = Instantiate(itemLabelPrefab, transform);
        itemLabelInstance.SetActive(false);
        itemLabelInstance.GetComponent<LabelController>().SetText(itemName, itemName[0]);
    }

    public void ShowLabel(bool show){
        itemLabelInstance.SetActive(show);
    }

    ~StockpileController(){
        sensorController.OnTagDetected -= ShowLabel;
    }
}
