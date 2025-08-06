using UnityEngine;

public class CompassUI : MonoBehaviour
{
    public ShipData shipData;
    RectTransform compassRect;

    void Awake()
    {
        compassRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Rotate compass so north is always up
        compassRect.localRotation = Quaternion.Euler(0, 0, -shipData.heading);
    }
}
