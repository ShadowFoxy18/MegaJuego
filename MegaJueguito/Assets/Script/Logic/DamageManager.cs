public class DamageManager
{
    public float CalculateDamage(AttackerStats atk, DefenderStats def)
    {
        float baseDamage = GetBaseDamage(atk);
        float resistenceMultiplier = GetResistenceMultiplier(atk, def);

        // El daño aditivo se suma AL FINAL, fuera de la multiplicación,
        // para que un "+15 fijo" no se vea reducido por la resistencia del defensor.
        return (baseDamage * resistenceMultiplier) + atk.additiveDamage + def.additiveDamage;
    }

    private float GetBaseDamage(AttackerStats atk)
    {
        return atk.attackType * atk.baseEscalator * atk.atqMultiplier * atk.multiplierDamage;
    }

    private float GetResistenceMultiplier(AttackerStats atk, DefenderStats def)
    {
        float defenseFactor = GetDefenseFactor(atk, def);
        float vulnerabilityFactor = GetVulnerabilityFactor(atk, def);
        float resistanceFactor = GetResistanceFactor(atk, def);

        return defenseFactor * vulnerabilityFactor * resistanceFactor;
    }

    private float GetDefenseFactor(AttackerStats atk, DefenderStats def)
    {
        return (atk.level + 20) /
            ((def.level + 20) * (0.1f + def.defense - def.reduction) * (atk.level + 20));
    }

    private float GetVulnerabilityFactor(AttackerStats atk, DefenderStats def)
    {
        return 1.0f - def.mitigation + atk.vulnerability;
    }

    private float GetResistanceFactor(AttackerStats atk, DefenderStats def)
    {
        return 1.0f - def.elementalRes + atk.penetrationRes;
    }
}