using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityListButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    AbilityData abilityData;
    public TMPro.TextMeshProUGUI buttonText;
    public GameObject tooltip;
    public TMPro.TextMeshProUGUI tooltipText;
    public void InitialiseButton(AbilityData data)
    {
        abilityData = data;
        buttonText.text = data.name;
        tooltipText.text = GetAbilityTooltip(data);
        tooltip.SetActive(false);
    }

    string GetAbilityTooltip(AbilityData ability)
    {
        return $"{ability.name}\nCost: {ability.energyCost} {ability.energyType}\nGenerates: {ability.generateAmount} {ability.generateType}\nDescription:\n {ability.description}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}
