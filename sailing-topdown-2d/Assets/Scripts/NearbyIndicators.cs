using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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

    // Existing API remains; uses GameObject.name
    public void AddPOI(GameObject poi)
    {
        AddPOI(poi, poi != null ? poi.name : "POI");
    }

    // New overload to allow a custom display name
    public void AddPOI(GameObject poi, string displayName)
    {
        if (poi == null) return;

        if (!poiIndicators.ContainsKey(poi))
        {
            var iconGO = Instantiate(poiIconPrefab, indicatorRoot);
            var iconRect = iconGO.GetComponent<RectTransform>();
            poiIndicators[poi] = iconRect;

            // Ensure this icon can receive pointer events (required for hover)
            EnsureRaycastTarget(iconGO);

            // Add or configure a tooltip on the icon
            var tooltip = iconGO.GetComponent<IndicatorTooltip>();
            if (tooltip == null) tooltip = iconGO.AddComponent<IndicatorTooltip>();
            tooltip.Initialize(displayName);
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

    static void EnsureRaycastTarget(GameObject iconGO)
    {
        // If there's no Graphic on the icon, add a transparent Image so it can receive pointer events.
        var hasGraphic = iconGO.GetComponent<Graphic>() != null || iconGO.GetComponentInChildren<Graphic>() != null;
        if (!hasGraphic)
        {
            var img = iconGO.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0); // invisible but raycast-targetable
            img.raycastTarget = true;
        }
    }
}

// Lightweight tooltip that appears above the indicator on hover.
// Requires an EventSystem in the scene.
sealed class IndicatorTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject tooltipGO;
    TextMeshProUGUI tooltipText;
    RectTransform tooltipRect;

    const float YOffset = 36f;

    public void Initialize(string text)
    {
        EnsureTooltipUI();
        SetText(text);
        Hide();
    }

    public void SetText(string text)
    {
        EnsureTooltipUI();
        tooltipText.text = text ?? string.Empty;
        // Optional: resize background if text is long
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    void Show()
    {
        if (tooltipGO != null)
            tooltipGO.SetActive(true);
    }

    void Hide()
    {
        if (tooltipGO != null)
            tooltipGO.SetActive(false);
    }

    void EnsureTooltipUI()
    {
        if (tooltipGO != null) return;

        // Create tooltip container
        tooltipGO = new GameObject("Tooltip", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        tooltipGO.transform.SetParent(transform, false);

        tooltipRect = tooltipGO.GetComponent<RectTransform>();
        tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
        tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
        tooltipRect.pivot = new Vector2(0.5f, 0f); // bottom-center
        tooltipRect.anchoredPosition = new Vector2(0f, YOffset);

        // Background
        var bg = tooltipGO.GetComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.75f);
        bg.raycastTarget = false;

        // Optional outline to help readability
        var outline = tooltipGO.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        outline.effectDistance = new Vector2(1f, -1f);

        // Create TMP text
        var textGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textGO.transform.SetParent(tooltipGO.transform, false);

        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(8f, 6f);   // padding
        textRect.offsetMax = new Vector2(-8f, -6f);

        tooltipText = textGO.GetComponent<TextMeshProUGUI>();
        tooltipText.alignment = TextAlignmentOptions.Center;
        tooltipText.color = Color.white;
        tooltipText.fontSize = 14f;
        tooltipText.textWrappingMode = TextWrappingModes.Normal;
        tooltipText.overflowMode = TextOverflowModes.Ellipsis;
        tooltipText.raycastTarget = false;

        // Reasonable default size; will grow/shrink based on content rebuild
        tooltipRect.sizeDelta = new Vector2(160f, 28f);
    }
}
