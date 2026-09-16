using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public abstract class ItemData : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    public bool isStackable = true;
    public Rarity rarity = Rarity.Common;
}