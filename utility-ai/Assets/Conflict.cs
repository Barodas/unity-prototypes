using UnityEngine;

public enum AttackType
{
    NONE,
    SOLIDCHOP,
    WEAKCHOP,
    SOLIDSTAB,
    WEAKSTAB,
    SHOOT
}

public enum DefendType
{
    NONE,
    SOLIDPARRY,
    WEAKPARRY
}

public enum SpecialOutcome
{
    NONE,
    DISARMATTACKER,
    DISARMDEFENDER,
    CRITICALDAMAGE,
    COUPDEGRACE
}

public static class Conflict
{
    /// <summary>
    /// Standard attacking conflict. 
    /// Will take the attacker and targets stats and perform an attack, then update values in each to reflect the results
    /// </summary>
    /// <param name="attacker">The Agent performing the attack</param>
    /// <param name="target">The Agent receiving the attack</param>
    public static void Attack(BaseAgent attacker, BaseAgent target)
    {
        // TODO: tally the stats of each agent and perform an attack
        // Print the outcome to the console for debugging

        float distanceBetweenAgents = Vector2.Distance(attacker.transform.position, target.transform.position);
        bool targetCanHurtAttacker = (target.Weapon != null ? target.Weapon.Range : 1) > distanceBetweenAgents;

        AttackType attackType = AttackType.NONE;
        if (!targetCanHurtAttacker)
            attackType = AttackType.SHOOT;
        else
        {
            attackType = (AttackType)Random.Range(1, 5);
        }

        int attackerDamage = (int)(attacker.Strength * (attacker.Weapon != null ? attacker.Weapon.DamageModifier : 1));

        DefendType defendType = (DefendType)Random.Range(0, 3);

        SpecialOutcome specialOutcome = ResolveSpecial(attackType, defendType);

        int defenceAmount = 0;

        switch (specialOutcome)
        {
            // TODO: Add cases

            default: // NONE
                if(attackType == AttackType.SHOOT)
                {
                    int success = Random.Range(0, 100);
                    if(success < 30 /*Accuracy?*/)
                    {
                        // Shoot Missed
                        break;
                    }

                    if(defendType == DefendType.WEAKPARRY)
                    {
                        // Weak parry against Shoot. Target takes a glancing hit. Target loses strength.
                        defenceAmount = (int)(attackerDamage * 0.5f);
                        target.AdjustStrength(-Random.Range(0, 2));
                    }
                    if(defendType == DefendType.SOLIDPARRY)
                    {
                        // Strong parry against Shoot. Target takes no hit. Target loses strength.
                        defenceAmount = attackerDamage;
                        target.AdjustStrength(-Random.Range(1, 4));
                    }
                }
                else
                {
                    if (defendType == DefendType.WEAKPARRY)
                    {
                        // Weak parry against Melee. Target takes a glancing hit. Target loses strength.
                        defenceAmount = (int)(attackerDamage * 0.5f);
                        target.AdjustStrength(-Random.Range(0, 2));
                    }
                    if (defendType == DefendType.SOLIDPARRY)
                    {
                        // Strong parry against Melee. Target takes no hit. Target loses strength.
                        defenceAmount = attackerDamage;
                        target.AdjustStrength(-Random.Range(1, 4));
                    }
                }
                break;
        }

        // Attacker loses strength based on attack type.
        if(attackType == AttackType.SOLIDCHOP || attackType == AttackType.SOLIDSTAB)
        {
            attacker.AdjustStrength(-Random.Range(0, 2));
        }
        if (attackType == AttackType.WEAKCHOP || attackType == AttackType.WEAKSTAB)
        {
            attacker.AdjustStrength(-Random.Range(1, 4));
        }

        // Apply damage
        int damage = (int)((attackerDamage - defenceAmount) * 0.1f);
        target.AdjustHealth(-damage);

        Debug.Log(string.Format("'{0}' attacked '{1}' with a '{2}'. '{1}' defends with a '{3}' and takes '{4}' damage.", attacker.Name, target.Name, attackType.ToString(), defendType.ToString(), damage.ToString()));
        // Where should the attacker/defender lose strength
        // create structs for attacker and defender
        // change attack types to separate type from weak/strong.
        // where does weapon durability adjust

        // Get the damage
        // Get the AttackType
        // Get the DefendType
        // SpecialOutcome
        // Get the Defend Result
        // Update Agents
        // Output to console
    }

    private static SpecialOutcome ResolveSpecial(AttackType attack, DefendType defence)
    {
        return SpecialOutcome.NONE;

        // TODO: return different outcomes based on input
    }
}
