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