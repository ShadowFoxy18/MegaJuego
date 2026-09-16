using UnityEngine;

public abstract class EnemyBrain : ITurnAgent
{
    public TurnAction DecideAction(BattleContext context)
    {
        string target = SelectTarget(context);
        TurnAction decision = BuildAction(target, context);

        Debug.Log($"{context.selfNickname} (EnemyBrain) decide: {decision.type} a {decision.targetNickname}");

        return decision;
    }

    // OBLIGATORIO: cada tipo de enemigo decide de su propia forma a quién apuntar
    protected abstract string SelectTarget(BattleContext context);


    protected virtual TurnAction BuildAction(string target, BattleContext context)
    {
        return new TurnAction { type = ActionType.Attack, targetNickname = target, skill = null };
    }
}