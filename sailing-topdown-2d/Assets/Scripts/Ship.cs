using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipData shipData;
    public NearbyIndicators nearbyIndicators;

    CircleCollider2D detectionCollider;

    void Start()
    {
        detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.radius = shipData.detectionRange;
        detectionCollider.isTrigger = true;

        shipData.Reset();
    }

    void Update()
    {
        // Update heading based on rudder
        shipData.UpdateHeading(shipData.rudderAngle, Time.deltaTime);

        // Move the ship forward in its local "up" direction
        float moveDirection = Mathf.Deg2Rad * (shipData.heading + 90f); // +90 so 0 = up
        Vector2 forward = new Vector2(Mathf.Cos(moveDirection), Mathf.Sin(moveDirection));
        transform.position += (Vector3)(forward * shipData.moveSpeed * Time.deltaTime);

        // Rotate the ship to match heading (so the sprite turns)
        transform.rotation = Quaternion.Euler(0, 0, shipData.heading);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PointOfInterest"))
        {
            Debug.Log("Point of Interest detected: " + col.name);
            if (nearbyIndicators != null)
            {
                nearbyIndicators.AddPOI(col.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("PointOfInterest"))
        {
            Debug.Log("Exited Point of Interest: " + col.name);
            if (nearbyIndicators != null)
            {
                nearbyIndicators.RemovePOI(col.gameObject);
            }
        }
    }
}
