using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringWheel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public ShipData shipData;

    RectTransform wheelRect;
    bool isDragging = false;
    float startAngle;
    float wheelStartAngle;
    float continuousWheelAngle = 0f;

    void Awake()
    {
        wheelRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(wheelRect, eventData.position, eventData.pressEventCamera, out localPoint);
        startAngle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
        wheelStartAngle = continuousWheelAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(wheelRect, eventData.position, eventData.pressEventCamera, out localPoint);
        float currentAngle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
        float angleDelta = Mathf.DeltaAngle(startAngle, currentAngle);
        float targetAngle = wheelStartAngle + angleDelta;

        float proposedAngle = continuousWheelAngle + Mathf.Clamp(
            Mathf.DeltaAngle(continuousWheelAngle, targetAngle),
            -shipData.wheelMaxRotationSpeed * Time.deltaTime,
            shipData.wheelMaxRotationSpeed * Time.deltaTime);

        float mappedRudder = Mathf.Clamp(proposedAngle * shipData.rudderMultiplier, -shipData.rudderMaxAngle, shipData.rudderMaxAngle);

        bool atLimit = Mathf.Abs(mappedRudder) >= shipData.rudderMaxAngle - 0.01f;
        bool movingTowardsCenter = Mathf.Sign(Mathf.DeltaAngle(continuousWheelAngle, targetAngle)) != Mathf.Sign(mappedRudder);

        if (!atLimit || movingTowardsCenter)
        {
            continuousWheelAngle = proposedAngle;
            wheelRect.localRotation = Quaternion.Euler(0, 0, continuousWheelAngle);

            if (shipData != null)
            {
                shipData.wheelAngle = continuousWheelAngle;
                shipData.rudderAngle = mappedRudder;
            }
        }
    }
    
    void Update()
    {
        if (!isDragging)
        {
            // Unwind the wheel to the target angle, preserving multiple rotations
            float returnTarget = shipData.wheelReturnTarget;
            float returnSpeed = shipData.wheelReturnSpeed;
            continuousWheelAngle = Mathf.MoveTowards(continuousWheelAngle, returnTarget, returnSpeed * Time.deltaTime);
            wheelRect.localRotation = Quaternion.Euler(0, 0, continuousWheelAngle);

            if (shipData != null)
            {
                shipData.wheelAngle = continuousWheelAngle;
                shipData.rudderAngle = Mathf.Clamp(continuousWheelAngle * shipData.rudderMultiplier, -shipData.rudderMaxAngle, shipData.rudderMaxAngle);
            }
        }
    }
}
