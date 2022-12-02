using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] loot;

    private List<Item> droppedItems = new List<Item>();

    private bool rolled = false;

    public void ShowLoot(Enemy owner)
    {
        if (!rolled) RollLoot();
        LootWindow.Instance.CreatePages(droppedItems, owner);
    }

    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);
            if (roll <= item.DropChance) droppedItems.Add(item.Item);
        }
        rolled = true;
    }
}
