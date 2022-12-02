using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItems : MonoBehaviour
{
    [SerializeField]
    private Item[] items;

    [SerializeField]
    private Chest chest;

    private void Start()
    {
        foreach (Item item in items)
        {
            List<SlotScript> slots = InventoryFrameScrit.Instance.GetAllItems();
            foreach (SlotScript slot in slots)
                item.Slot = chest.Inventory.Slots.Find(x => x.Index == slot.Index);
            chest.Items.Add(item);
        }
    }
}
