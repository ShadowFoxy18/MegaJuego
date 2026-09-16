using UnityEngine;

public interface ITurnAgent
{
    TurnAction DecideAction(BattleContext cntx);
}
