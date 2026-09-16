using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    private static ItemDatabase _instance;

    public static ItemDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ItemDatabase>("ItemDatabase");
            }
            return _instance;
        }
    }

    public List<ItemData> allItems;

    public ItemData GetById(int id)
    {
        return allItems.FirstOrDefault(item => item.itemId == id);
    }
}