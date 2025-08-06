using UnityEngine;

public class Rudder : MonoBehaviour
{
    public ShipData shipData;
    
    void Update()
    {
        if (shipData != null)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -shipData.rudderAngle); // Invert angle to match steering direction
        }
    }
}
