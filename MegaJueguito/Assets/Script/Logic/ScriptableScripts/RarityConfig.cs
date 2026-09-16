using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

[System.Serializable]
public class RarityColorEntry
{
    public Rarity rarity;
    public Color color = Color.white;
}

[CreateAssetMenu(menuName = "Items/RarityConfig")]
public class RarityConfig : ScriptableObject
{
    public List<RarityColorEntry> colors;

    public Color GetColor(Rarity rarity)
    {
        RarityColorEntry entry = colors.FirstOrDefault(c => c.rarity == rarity);
        return entry != null ? entry.color : Color.white;
    }
}