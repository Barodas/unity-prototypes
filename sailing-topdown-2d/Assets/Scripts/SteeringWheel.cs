using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    public ShipData shipData;
    
    bool isDragging = false;
    float startAngle;
    float wheelStartAngle;
    float continuousWheelAngle = 0f;

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPos - transform.position;
        startAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        wheelStartAngle = transform.eulerAngles.z;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mouseWorldPos - transform.position;
            float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angleDelta = Mathf.DeltaAngle(startAngle, currentAngle);
            float targetAngle = wheelStartAngle + angleDelta;

            // Calculate the proposed new wheel angle
            float proposedAngle = continuousWheelAngle + Mathf.Clamp(
                Mathf.DeltaAngle(continuousWheelAngle, targetAngle),
                -shipData.wheelMaxRotationSpeed * Time.deltaTime,
                shipData.wheelMaxRotationSpeed * Time.deltaTime);

            // Map to rudder and clamp
            float mappedRudder = Mathf.Clamp(proposedAngle * shipData.rudderMultiplier, -shipData.rudderMaxAngle, shipData.rudderMaxAngle);

            // Only update if the rudder is not at its limit, or if moving back towards center
            bool atLimit = Mathf.Abs(mappedRudder) >= shipData.rudderMaxAngle - 0.01f;
            bool movingTowardsCenter = Mathf.Sign(Mathf.DeltaAngle(continuousWheelAngle, targetAngle)) != Mathf.Sign(mappedRudder);

            if (!atLimit || movingTowardsCenter)
            {
                continuousWheelAngle = proposedAngle;
                transform.rotation = Quaternion.Euler(0, 0, continuousWheelAngle);

                if (shipData != null)
                {
                    shipData.wheelAngle = continuousWheelAngle;
                    shipData.rudderAngle = mappedRudder;
                }
            }
        }
        else
        {
            // Linearly unwind the wheel to the target angle, preserving multiple rotations
            float returnTarget = shipData.wheelReturnTarget;
            float returnSpeed = shipData.wheelReturnSpeed;
            continuousWheelAngle = Mathf.MoveTowards(continuousWheelAngle, returnTarget, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, continuousWheelAngle);

            if (shipData != null)
            {
                shipData.wheelAngle = continuousWheelAngle;
                shipData.rudderAngle = Mathf.Clamp(continuousWheelAngle * shipData.rudderMultiplier, -shipData.rudderMaxAngle, shipData.rudderMaxAngle);
            }
        }
    }
}
