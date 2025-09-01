/// <summary>
/// An object that is capable of taking damage and dying/being destroyed.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Should handle increase or decrease of health and call die/destroy if applicable.
    /// </summary>
    void AdjustHealth(int amount);

    /// <summary>
    /// Objects health has reached 0. Typically removes Object from behaviour update. 
    /// Possibility of ressurection/repair, or object remains relevant to living entities in some way.
    /// </summary>
    void Die();

    /// <summary>
    /// Object is utterly destroyed. Typically deletes the GameObject.
    /// </summary>
    void Destroy();
}
