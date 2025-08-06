using System.Collections.Generic;
using UnityEngine;

public class NearbyIndicators : MonoBehaviour
{
    public Transform shipTransform;
    public GameObject poiIconPrefab;
    public float indicatorRadius = 200f;

    RectTransform indicatorRoot;
    Dictionary<GameObject, RectTransform> poiIndicators = new Dictionary<GameObject, RectTransform>();

    void Start()
    {
        indicatorRoot = GetComponent<RectTransform>();
    }

    void Update()
    {
        float shipHeading = shipTransform.eulerAngles.z;

        foreach (var poi in poiIndicators.Keys)
        {
            Vector2 worldDir = (poi.transform.position - shipTransform.position).normalized;

            // Rotate direction by negative ship heading so UI "up" matches ship's forward
            float angleToPOI = Mathf.Atan2(worldDir.y, worldDir.x) * Mathf.Rad2Deg - shipHeading;

            Vector2 uiPos = new Vector2(
                Mathf.Cos(angleToPOI * Mathf.Deg2Rad),
                Mathf.Sin(angleToPOI * Mathf.Deg2Rad)
            ) * indicatorRadius;

            poiIndicators[poi].anchoredPosition = uiPos;
        }
    }
    
    public void AddPOI(GameObject poi)
    {
        if (!poiIndicators.ContainsKey(poi))
        {
            var icon = Instantiate(poiIconPrefab, indicatorRoot);
            poiIndicators[poi] = icon.GetComponent<RectTransform>();
        }
    }

    public void RemovePOI(GameObject poi)
    {
        if (poiIndicators.TryGetValue(poi, out var icon))
        {
            Destroy(icon.gameObject);
            poiIndicators.Remove(poi);
        }
    }
}
