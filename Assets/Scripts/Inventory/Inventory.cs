using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Prefab for creating slots
    /// </summary>
    [SerializeField]
    private GameObject slotPrefab;

    /// <summary>
    /// A canvasgroup for hiddin and showing the inventory bag
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// A list of all the slots the belongs to the inventory bag
    /// </summary>
    private List<SlotScript> slots = new List<SlotScript>();

    public int Index { get; set; }

    /// <summary>
    /// Indicates if this bag is open or closed
    /// </summary>
    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }
    }

    public List<SlotScript> Slots { get => slots; }

    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            foreach(SlotScript slot in Slots) if (slot.IsEmpty) count++;
            return count;
        }
    }
     

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();
        foreach(SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.Items) items.Add(item);
            }
        }
        return items;
    }

    /// <summary>
    /// Creates slots for this inventory bag
    /// </summary>
    /// <param name="slotsCount">Amount of slots to create</param>
    public void AddSlots(int slotsCount)
    {
        for (int i = 0; i < slotsCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.Index = i;
            slot.Inventory = this;
            Slots.Add(slot);
        }
    }

    /// <summary>
    /// Adds an item to the inventory bag
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in Slots) 
        {
            if(slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Opens or closes inventory bag
    /// </summary>
    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void Clear()
    {
        foreach (SlotScript slot in slots) slot.Clear();
    }
}
