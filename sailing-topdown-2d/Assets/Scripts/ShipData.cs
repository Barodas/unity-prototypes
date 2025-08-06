using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    public float rudderMaxAngle = 30f; // Maximum rudder angle in degrees
    public float rudderMultiplier = 0.2f; // Rudder turns at a different rate than the wheel
    public float wheelMaxRotationSpeed = 180f; // Maximum rotation speed in degrees per second
    public float wheelReturnTarget = 0f; // Target angle for the wheel to return to when not being dragged
    public float wheelReturnSpeed = 90f; // Speed at which the wheel returns to the target angle
    public float turnRate = 30f; // Degrees per second per max rudder
    public float detectionRange = 100f; // Range for detecting obstacles

    public float rudderAngle;
    public float wheelAngle;
    public float moveSpeed;
    public float heading;

    public void Reset()
    {
        rudderAngle = 0f;
        wheelAngle = 0f;
        moveSpeed = 0f;
        heading = 0f;
    }

    public void UpdateHeading(float rudderAngle, float deltaTime)
    {
        // Rudder angle is clamped, so this is safe
        heading += rudderAngle * turnRate * deltaTime;
        heading = Mathf.Repeat(heading, 360f); // Keep heading in [0, 360)
    }
}
