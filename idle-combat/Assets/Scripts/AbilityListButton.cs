using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityListButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    AbilityData abilityData;
    public Button button;
    public TMPro.TextMeshProUGUI buttonText;
    public GameObject tooltip;
    public TMPro.TextMeshProUGUI tooltipText;
    public void InitialiseButton(AbilityData data, UnityEngine.Events.UnityAction action)
    {
        abilityData = data;
        buttonText.text = data.name;
        tooltipText.text = GetAbilityTooltip(data);
        button.onClick.AddListener(action);
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
