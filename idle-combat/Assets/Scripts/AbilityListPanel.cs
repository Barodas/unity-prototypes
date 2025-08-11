using System.Collections.Generic;
using UnityEngine;

public class AbilityListPanel : MonoBehaviour
{
    public RectTransform panelContainer;
    public ActiveAbilityPanel activeAbilityPanel;
    public GameObject abilityButtonPrefab;
    public List<AbilityData> allAbilities;

    void Start()
    {
        PopulateAbilityPanel();
    }

    void PopulateAbilityPanel()
    {
        foreach (var ability in allAbilities)
        {
            var buttonObj = Instantiate(abilityButtonPrefab, panelContainer);
            var abilityListButton = buttonObj.GetComponentInChildren<AbilityListButton>();
            abilityListButton.InitialiseButton(ability, () => activeAbilityPanel.AddAbility(ability));
        }
    }
}