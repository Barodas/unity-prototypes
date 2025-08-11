using System.Collections.Generic;
using UnityEngine;

public class AbilityListPanel : MonoBehaviour
{
    public RectTransform panelContainer; // Assign in inspector: the panel anchored bottom right
    public GameObject abilityButtonPrefab; // Assign in inspector: prefab with Button + Tooltip
    public List<AbilityData> allAbilities; // Assign in inspector or load dynamically

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
            abilityListButton.InitialiseButton(ability);
        }
    }
}