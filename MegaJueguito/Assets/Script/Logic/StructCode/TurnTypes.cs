using System.Collections.Generic;

public enum Team
{
    Protagonist,
    Antagonist,
    None
}

public interface ITurnParticipant
{
    float Speed { get; }
}

[System.Serializable]
public struct ActionValueData
{
    public float baseAV;
    public float currentAV;

    public ActionValueData(float speed)
    {
        baseAV = TurnQueue.BASE_ACTION_GAUGE / speed;
        currentAV = baseAV;
    }
}

public enum ActionType
{
    Attack,
    Guard,
    Investigate,
    UseItem,
    Flee
}

public struct TurnAction
{
    public ActionType type;

    public string targetNickname; 
    public SkillData skill;       
}

public class BattleContext
{
    public string selfNickname;
    public List<CombatUnit> allies;
    public List<string> enemyNicknames;
    public CombatDirector director;
}