using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform environmentRoot;

    void Update()
    {
        // Rotate compass to counteract environment rotation
        transform.rotation = Quaternion.Euler(0, 0, -environmentRoot.eulerAngles.z);
    }
}
