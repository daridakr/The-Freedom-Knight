using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryFrameScrit : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryFrameScrit instance;

    public static InventoryFrameScrit Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<InventoryFrameScrit>();
            return instance;
        }
    }

    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    // For debuggin
    [SerializeField]
    private Item[] items;

    
    public bool CanAddBag
    {
        get { return Bags.Count < 4; }
    }

    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            foreach(Bag bag in Bags) count += bag.InventoryScript.EmptySlotCount;
            return count;
        }
    }

    public int TotalSlotCount
    {
        get
        {
            int count = 0;
            foreach(Bag bag in Bags) count += bag.InventoryScript.Slots.Count;
            return count;
        }
    }

    public int FullSlotCount
    {
        get
        {
            return TotalSlotCount - EmptySlotCount;
        }
    }


    public SlotScript FromSlot 
    {
        get { return fromSlot; } 
        set
        {
            fromSlot = value;
            if (value != null) fromSlot.Icon.color = Color.grey;
        }
    }

    public List<Bag> Bags { get => bags; }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(8);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(18);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(18);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem((Armory)Instantiate(items[2]));
            AddItem((Armory)Instantiate(items[3]));
            AddItem((Armory)Instantiate(items[4]));
            AddItem((Armory)Instantiate(items[5]));
            AddItem((Armory)Instantiate(items[6]));
            AddItem((Armory)Instantiate(items[7]));
        }
    }

    /// <summary>
    /// Equips a bag to the inventory
    /// </summary>
    /// <param name="bag"></param>
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                Bags.Add(bag);
                bag.BagButton = bagButton;
                bag.InventoryScript.transform.SetSiblingIndex(bagButton.BagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)
    {
        Bags.Add(bag);
        bagButton.Bag = bag;
        bag.InventoryScript.transform.SetSiblingIndex(bagButton.BagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetUpScript();
        Bags.Add(bag);
        bag.BagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].Bag = bag;
    }

    /// <summary>
    /// Removes the bag from the inventory
    /// </summary>
    /// <param name="bag"></param>
    public void RemoveBag(Bag bag)
    {
        Bags.Remove(bag);
        Destroy(bag.InventoryScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (TotalSlotCount - oldBag.SlotCount) + newBag.SlotCount;
        if (newSlotCount - FullSlotCount >= 0)
        {
            List<Item> bagItems = oldBag.InventoryScript.GetItems();
            RemoveBag(oldBag);
            newBag.BagButton = oldBag.BagButton;
            newBag.Use();
            foreach (Item item in bagItems) if (item != newBag) AddItem(item); // if not dupclicates
            AddItem(oldBag);
            HandScript.Instance.Drop();
            Instance.fromSlot = null;
        }
    }

    /// <summary>
    /// Adds an item to the inventory 
    /// </summary>
    /// <param name="item">Item to add</param>
    public bool AddItem(Item item)
    {
        if (item.StackSize > 0) if (PlaceInStack(item)) return true;
        return PlaceInEmpty(item);
    }

    /// <summary>
    /// Places an item on an empty slot in the game
    /// </summary>
    /// <param name="item">Item we are trying to add</param>
    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in Bags)
        {
            if (bag.InventoryScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Tries to stack an item on another
    /// </summary>
    /// <param name="item">Item we try to stack</param>
    /// <returns></returns>
    private bool PlaceInStack(Item item)
    {
        foreach(Bag bag in Bags)
        {
            foreach(SlotScript slots in bag.InventoryScript.Slots)
            {
                if (slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].InventoryScript.Slots[slotIndex].AddItem(item);
    }

    /// <summary>
    /// Closes all bags but current
    /// </summary>
    /// <param name="currentBag"></param>
    public void OpenClose(Bag currentBag)
    {
        var openBags = Bags.Select(x => x).Where(x => x.InventoryScript.IsOpen);
        foreach (var bag in openBags) if (bag != currentBag) bag.InventoryScript.OpenClose();
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.InventoryScript.Slots)
                if (!slot.IsEmpty) slots.Add(slot);
        }
        return slots;
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.InventoryScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.GetType() == type.GetType())
                {
                    foreach (Item item in slot.Items) useables.Push(item as IUseable);
                }
            }
        }
        return useables;
    }

    public IUseable GetUseable(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.InventoryScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Title == type) return (slot.Item as IUseable);
            }
        }
        return null;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.InventoryScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Title == type) itemCount += slot.Items.Count;
            }
        }
        return itemCount;
    }

    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();
        foreach (Bag bag in Bags)
        {
            foreach (SlotScript slot in bag.InventoryScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.Title == type)
                {
                    foreach (Item item in slot.Items)
                    {
                        items.Push(item);
                        if (items.Count == count) return items;
                    }
                }
            }
        }
        return items;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null) itemCountChangedEvent.Invoke(item);
    }
}
