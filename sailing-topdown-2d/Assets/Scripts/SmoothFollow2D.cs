using UnityEngine;

public class SmoothFollow2D : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, -0.8f, -10f);

    [Tooltip("Keep the target in the same position on screen.")]
    public bool rotateOffsetWithTarget = false;

    [Tooltip("Time for the camera to catch up to the target position.")]
    public float positionSmoothTime = 0.15f;

    [Tooltip("Time for the camera to catch up to the target rotation (Z).")]
    public float rotationSmoothTime = 0.25f;

    Vector3 positionVelocity;
    float angularVelocity;

    void LateUpdate()
    {
        if (target == null) return;

        float targetZ = target.eulerAngles.z;
        float currentZ = transform.eulerAngles.z;
        float smoothedZ = Mathf.SmoothDampAngle(currentZ, targetZ, ref angularVelocity, rotationSmoothTime);

        Vector3 desiredOffset = rotateOffsetWithTarget
            ? (Quaternion.Euler(0f, 0f, smoothedZ) * offset)
            : offset;

        Vector3 desiredPos = target.position + desiredOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref positionVelocity, positionSmoothTime);

        transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
    }
}