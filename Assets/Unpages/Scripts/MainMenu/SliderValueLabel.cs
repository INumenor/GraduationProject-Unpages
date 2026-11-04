using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shows a slider's value as a percentage of its range.
public class SliderValueLabel : MonoBehaviour
{
    public Slider slider;
    public TMP_Text label;

    void OnEnable()
    {
        slider.onValueChanged.AddListener(Refresh);
        Refresh(slider.value);
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(Refresh);
    }

    void Refresh(float value)
    {
        label.text = Mathf.RoundToInt(slider.normalizedValue * 100f) + "%";
    }
}
