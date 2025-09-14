using UnityEngine;
using UnityEngine.UI;

public class SpeedThrottleUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ShipData shipData;

    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private Text valueText; // Optional: displays the current speed

    [Header("Speed Range")]
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Behavior")]
    [SerializeField] private bool topIsMax = true; // If false, top is min (invert)

    private void Reset()
    {
        slider = GetComponent<Slider>();
    }

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
        ConfigureSlider();
        // Do NOT sync value here; can cause SendMessage warnings during initialization.
    }

    private void Start()
    {
        // Safe time to push value into the UI
        SyncFromData();
    }

    private void OnEnable()
    {
        if (slider != null) slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDisable()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void Update()
    {
        // If another system changes the speed, keep the UI in sync without feedback loops
        if (shipData != null && slider != null)
        {
            float clamped = Mathf.Clamp(shipData.moveSpeed, minSpeed, maxSpeed);
            if (!Mathf.Approximately(slider.value, clamped))
                slider.SetValueWithoutNotify(clamped);

            UpdateLabel(clamped);
        }
    }

    private void OnValidate()
    {
        if (maxSpeed < minSpeed) maxSpeed = minSpeed;
        if (slider != null) ConfigureSlider();
        // Do not push values to the Slider here.
    }

    private void ConfigureSlider()
    {
        // Make the slider vertical and set direction
        slider.direction = topIsMax ? Slider.Direction.BottomToTop : Slider.Direction.TopToBottom;
        slider.minValue = minSpeed;
        slider.maxValue = maxSpeed;
        slider.wholeNumbers = false;
    }

    private void SyncFromData()
    {
        // Prevent UI mutations during validation/edit-time
        if (!Application.isPlaying) return;

        if (shipData == null || slider == null) return;
        float v = Mathf.Clamp(shipData.moveSpeed, minSpeed, maxSpeed);
        slider.SetValueWithoutNotify(v);
        UpdateLabel(v);
    }

    private void OnSliderChanged(float value)
    {
        if (shipData == null) return;
        shipData.moveSpeed = Mathf.Clamp(value, minSpeed, maxSpeed);
        UpdateLabel(shipData.moveSpeed);
    }

    private void UpdateLabel(float value)
    {
        if (valueText != null)
            valueText.text = $"{value:0.0}";
    }
}