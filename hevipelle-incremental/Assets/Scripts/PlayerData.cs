using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Starting Stats")]
    [SerializeField] private int _startingFaith = 10;
    [SerializeField] private int _startingPopulation = 10;
    [SerializeField] private int _startingPopulationCap = 10;
    [SerializeField] private int _startingDivinity = 0;
    [SerializeField] private float _startingTickInterval = 1.0f;

    [Header("Current Stats")]
    public int Faith;
    public int Population;
    public int PopulationCap;
    public int Divinity;
    public float TickInterval;

    public delegate void OnStatsChanged();
    public event OnStatsChanged StatsChanged;

    public void Reset()
    {
        Faith = _startingFaith;
        Population = _startingPopulation;
        PopulationCap = _startingPopulationCap;
        Divinity = _startingDivinity;
        TickInterval = _startingTickInterval;
    }

    public void SetPopulationCap(int newCap)
    {
        PopulationCap = newCap + _startingPopulationCap;
        StatsChanged?.Invoke();
    }

    public void Tick()
    {
        // TODO: Determine how Divinity affects faith gain
        Population = Mathf.Clamp(Population + (int)(Population * 0.3f), 0, PopulationCap);
        Faith += Population;
        StatsChanged?.Invoke();
    }
}
