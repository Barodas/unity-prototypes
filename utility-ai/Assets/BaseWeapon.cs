using System;

public class BaseWeapon : IDamageable
{
    /// <summary>
    /// Used by IDamageable AdjustHealth.
    /// Modifies the damage of the weapon similar to DamageModifier.
    /// Weapon is usually destroyed when Durability reaches 0;
    /// </summary>
    public int Durability { get; private set; }

    /// <summary>
    /// Range of the Weapon (1 = Melee Short (ie. Dagger), 100 = Siege Long (ie. Trebuchet).
    /// </summary>
    public int Range { get; private set; }

    /// <summary>
    /// Melee: Modifies the strength of the user to determine damage.
    /// Ranged: Modifies the damage of the projectile fired by the weapon.
    /// </summary>
    public float DamageModifier { get; private set; }



    #region IDamageable Implementation

    public void AdjustHealth(int amount)
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        throw new NotImplementedException();
    }

    #endregion // IDamageable Implementation
}
