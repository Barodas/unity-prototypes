using UnityEngine;

public class Rudder : MonoBehaviour
{
    public SteeringData steeringData;
    
    void Update()
    {
        if (steeringData != null)
        {
            transform.rotation = Quaternion.Euler(0, 0, -steeringData.rudderAngle); // Invert angle to match steering direction
        }
    }
}
