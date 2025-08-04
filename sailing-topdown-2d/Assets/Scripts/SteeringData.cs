using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/SteeringData")]
public class SteeringData : ScriptableObject
{
    public float rudderMaxAngle = 30f; // Maximum rudder angle in degrees
    public float rudderMultiplier = 0.2f; // Rudder turns at a different rate than the wheel
    public float maxRotationSpeed = 180f; // Maximum rotation speed in degrees per second
    public float returnTarget = 0f; // Target angle for the wheel to return to when not being dragged
    public float returnSpeed = 90f; // Speed at which the wheel returns to the target angle

    public float rudderAngle;
    public float wheelAngle;
}
