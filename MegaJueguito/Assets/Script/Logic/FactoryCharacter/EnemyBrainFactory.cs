using UnityEngine;

// Comportamiento por defecto: usado como base y como respaldo para
// cualquier alineamiento que todavía no tenga su propia clase concreta.
public class BasicAggressiveBrain : EnemyBrain
{
    protected override string SelectTarget(BattleContext context)
    {
        return context.enemyNicknames.Count > 0 ? context.enemyNicknames[0] : null;
    }
}

public class LawfulEvilBrain : EnemyBrain
{
    protected override string SelectTarget(BattleContext context)
    {
        return context.enemyNicknames.Count > 0 ? context.enemyNicknames[0] : null;
    }

    protected override TurnAction BuildAction(string target, BattleContext context)
    {
        // TODO: cuando SkillData tenga contenido real, elegir acá una habilidad
        // "no letal"/de incapacitar en vez del ataque normal — tiene la capacidad
        // de matar, pero elige no hacerlo.
        return base.BuildAction(target, context);
    }
}

public class ChaoticEvilBrain : EnemyBrain
{
    protected override string SelectTarget(BattleContext context)
    {
        if (context.enemyNicknames.Count == 0) return null;
        int index = Random.Range(0, context.enemyNicknames.Count);
        return context.enemyNicknames[index];
    }
}

public static class EnemyBrainFactory
{
    public static EnemyBrain Create(Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.LawfulEvil:
                return new LawfulEvilBrain();

            case Alignment.ChaoticEvil:
                return new ChaoticEvilBrain();

            // TODO: agregar el resto de los 9 alineamientos con su propio comportamiento.
            // Mientras tanto, cualquiera sin clase propia usa el comportamiento básico.
            default:
                return new BasicAggressiveBrain();
        }
    }
}