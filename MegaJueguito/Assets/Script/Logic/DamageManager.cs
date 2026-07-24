using UnityEngine;

public class DamageManager
{
    BaseDamage baseDamage;
    ResistenceMultiplier resistenceMultiplier;
    ElementalMultiplier elementalMultiplier;


    class BaseDamage : DamageManager
    {
        float baseEscalator;
        float atqMultiplier;
        float MultiplierDamage;
        float additiveDamage;

        public float GetBaseDamage()
        {
            return (baseEscalator * atqMultiplier * MultiplierDamage) + additiveDamage;
        }
    }
    class ResistenceMultiplier : DamageManager
    {
        int defLevel;
        float def_Defense;
        float def_Reduction;
        float def_Mitigation;
        float def_ElementalRes;

        int atqLevel;
        float atq_Vulnerability;
        float atq_PENRes;

        public float GetResistenceMultiplier()
        {
            float defense = GetDefense();
            float vulnerability = GetVulnerability();
            float resistance = GetResistence();

            return (defense * vulnerability * resistance);
        }

        float GetDefense()
        {
            return (atqLevel + 20) / ((defLevel + 20) * (0.1f + def_Defense - def_Reduction) * (atqLevel + 20));
        }

        float GetVulnerability()
        { 
            return 1.0f - def_Mitigation + atq_Vulnerability;
        }

        float GetResistence()
        {
            return 1.0f - def_ElementalRes + atq_PENRes;
        }
    }
    class ElementalMultiplier : DamageManager
    {
        float aditionalBonus;

        int maestry;
        float anomaly;
        float elementalBonus;

        public float GetElementalMultiplier()
        {
            float maestryMultiplier = GetMaestryMultiplier();
            float bonoElemental = GetBonoElemental();

            return maestryMultiplier * (bonoElemental + 1) * (1 + aditionalBonus);
        }

        float GetMaestryMultiplier()
        {
            return ((maestry * 2) / (maestry + 1200)) * (1 + anomaly);
        }

        float GetBonoElemental()
        {
            return (((5 + maestry) * (1 + elementalBonus) * 0.6f) / (maestry + 1250)) * ((1 + elementalBonus) * 12);
        }
    }

    float CalculateDamage()
    {
        float baseDamage = this.baseDamage.GetBaseDamage();
        float resistenceMultiplier = this.resistenceMultiplier.GetResistenceMultiplier();
        float elementalMultiplier = this.elementalMultiplier.GetElementalMultiplier();
        return baseDamage * resistenceMultiplier * elementalMultiplier;
    }
}
