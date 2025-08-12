using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveAbilitySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button removeButton;
    public TextMeshProUGUI slotName;
    public TextMeshProUGUI slotInfo;
    public Image abilityIcon;
    private AbilityData ability;
    [SerializeField]private int slotIndex;
    private ActiveAbilityPanel panel;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;

    public void Setup(ActiveAbilityPanel panel, int index)
    {
        this.panel = panel;
        this.slotIndex = index;
        removeButton.onClick.AddListener(() => panel.RemoveAbility(slotIndex));
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetAbility(AbilityData ability)
    {
        if(ability != null)
        {
            this.ability = ability;
            abilityIcon.enabled = true;
            abilityIcon.sprite = (ability.icon != null) ? ability.icon : null;
            slotName.enabled = true;
            slotName.text = ability.name;
            slotInfo.enabled = true;
            slotInfo.text = GetAbilityInfo(ability);
            removeButton.gameObject.SetActive(true);
        }
        else
        {
            this.ability = null;
            abilityIcon.enabled = false;
            slotName.text = $"Slot {slotIndex + 1}";
            slotName.enabled = false;
            slotInfo.text = "Empty Slot";
            slotInfo.enabled = false;
            removeButton.gameObject.SetActive(false);
        }
    }

    private string GetAbilityInfo(AbilityData ability)
    {
        return $"Cost:\n {ability.energyCost} {ability.energyType}\nGenerates:\n {ability.generateAmount} {ability.generateType}";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ability == null) return;
        originalPosition = rectTransform.position;
        originalParent = transform.parent;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(panel.slotContainer.parent); // Move to top canvas for drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ability == null) return;
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ability == null) return;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        rectTransform.position = originalPosition;

        // Check for drop on another slot
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            var targetSlot = result.gameObject.GetComponent<ActiveAbilitySlot>();
            if (targetSlot != null && targetSlot != this)
            {
                panel.MoveAbility(slotIndex, targetSlot.slotIndex);
                break;
            }
        }
    }
}