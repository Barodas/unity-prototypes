using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilitySlot : MonoBehaviour
{
    public Button removeButton;
    public TextMeshProUGUI slotName;
    public TextMeshProUGUI slotInfo;
    public Image abilityIcon;
    private AbilityData ability;
    private int slotIndex;
    private ActiveAbilityPanel panel;

    public void Setup(ActiveAbilityPanel panel, int index)
    {
        this.panel = panel;
        this.slotIndex = index;
        removeButton.onClick.AddListener(() => panel.RemoveAbility(slotIndex));
        // Add drag-and-drop event handlers here
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
    // Implement drag-and-drop logic here
}