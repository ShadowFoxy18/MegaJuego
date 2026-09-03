using UnityEngine;

public struct AttackerStats
{
    public int level;
    public float attackType;         // Character._attack
    public float maestry;        // Character._maestry
    public float anomaly;        // Character._anomaly
    public float vulnerability;  // 0f por ahora, pendiente sistema de estados activos
    public float penetrationRes; // 0f por ahora, pendiente sistema de estados activos

    public float baseEscalator;    // viene de SkillData
    public float atqMultiplier;    // viene de SkillData
    public float multiplierDamage; // viene de SkillData

    public float additiveDamage; // suma de: arma + reacciones propias activas (pendiente)
}

public struct DefenderStats
{
    public int level;
    public float defense;      // Character._defense
    public float mitigation;   // Character._mitigation
    public float reduction;    // 0f por ahora, pendiente sistema de estados activos
    public float elementalRes; // Character._resistances[elementoDelAtaque]

    public float additiveDamage; // suma de estados activos que aumentan daño recibido (ej: Enredo), pendiente
}