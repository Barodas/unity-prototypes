using UnityEngine;

public class EdgeColliderController : MonoBehaviour
{
    public Camera Camera;
    public EdgeCollider2D LeftEdge;
    public EdgeCollider2D RightEdge;

	void Start ()
    {
        Vector2[] left = new Vector2[2];
        left[0] = Camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        left[1] = Camera.ScreenToWorldPoint(new Vector3(0, Camera.pixelHeight, 0));
        LeftEdge.points = left;

        Vector2[] right = new Vector2[2];
        right[0] = Camera.ScreenToWorldPoint(new Vector3(Camera.pixelWidth, 0, 0));
        right[1] = Camera.ScreenToWorldPoint(new Vector3(Camera.pixelWidth, Camera.pixelHeight, 0));
        RightEdge.points = right;

        Debug.Log(left[0] + " - " + left[1]);
        Debug.Log(right[0] + " - " + right[1]);
    }

	void Update ()
    {
		
	}
}
