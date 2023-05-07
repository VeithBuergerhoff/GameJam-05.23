using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskOverlayEntry : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textField;

    [SerializeField]
    public Slider timeSlider;

    [SerializeField]
    public Image sliderFill;

    [SerializeField]
    public Gradient timeGradient;
}