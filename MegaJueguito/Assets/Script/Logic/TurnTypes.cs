public enum Team
{
    Protagonist,
    Antagonist
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
        baseAV = TurnManager.BASE_ACTION_GAUGE / speed;
        currentAV = baseAV;
    }
}