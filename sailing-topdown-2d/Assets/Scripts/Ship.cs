using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipData shipData;
    public Transform environmentRoot;

    void Update()
    {
        // Calculate movement direction based on rudder angle
        float moveDirection = Mathf.Deg2Rad * shipData.rudderAngle;
        Vector2 forward = new Vector2(Mathf.Cos(moveDirection), Mathf.Sin(moveDirection));

        // Move the environment opposite to the ship's forward direction
        environmentRoot.position -= (Vector3)(forward * shipData.moveSpeed * Time.deltaTime);

        // Rotate environment opposite to ship's heading (rudderAngle or heading)
        shipData.UpdateHeading(shipData.rudderAngle, Time.deltaTime);
        environmentRoot.rotation = Quaternion.Euler(0, 0, shipData.heading);
    }
}
