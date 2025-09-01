using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float RotationMultiplier = 10;

    private float _curZoomDistance;
    private bool _isRotating;
    private Vector3 _rotationOrigin;
    private Vector3 _prevMousePosition;

	void Start ()
    {
	    
	}
	
	void LateUpdate ()
    {
        if(_isRotating)
        {
            Vector3 curMousePosition = Input.mousePosition;
            Vector3 offset = curMousePosition - _prevMousePosition;

            Vector3 targetPosition = transform.TransformPoint(offset);
            transform.position = CalculatePosition(_rotationOrigin, targetPosition, _curZoomDistance);
            transform.LookAt(_rotationOrigin);

            Debug.Log("offset: " + offset + ", target: " + targetPosition);

            _prevMousePosition = curMousePosition;
        }

	    if(Input.GetMouseButtonDown(1))
        {
            _isRotating = true;
            _prevMousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                _rotationOrigin = hit.point;
                _curZoomDistance = Vector3.Distance(hit.point, transform.position);
            }
            else
            {
                Debug.Log("Failed to find valid _rotationOrigin");
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            _isRotating = false;
        }
	}

    private Vector3 CalculatePosition(Vector3 origin, Vector3 target, float maxDistance)
    {
        Ray ray = new Ray(origin, (target - origin).normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            return hit.point;
        }
        else
        {
            return ray.GetPoint(maxDistance);
        }
    }
}
