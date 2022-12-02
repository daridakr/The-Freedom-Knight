using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// A stack for all items on this slot
    /// </summary>
    private ObservableStack<Item> items = new ObservableStack<Item>();

    /// <summary>
    /// A reference to the slot's icon
    /// </summary>
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    /// <summary>
    /// A reference to the inventory that this slot belong to
    /// </summary>
    public Inventory Inventory { get; set; }

    public int Index { get; set; }

    /// <summary>
    /// Checks if the item is empty
    /// </summary>
    public bool IsEmpty
    {
        get { return Items.Count == 0; }
    }

    /// <summary>
    /// Indicates if the slot is full
    /// </summary>
    public bool IsFull
    { 
        get
        {
            if (IsEmpty || Count < Item.StackSize) return false;
            return true;
        }
    }

    /// <summary>
    /// The current item on the slot
    /// </summary>
    public Item Item
    {
        get
        {
            if (!IsEmpty) return Items.Peek();
            return null;
        }
    }

    /// <summary>
    /// The icon on the slot
    /// </summary>
    public Image Icon { get => icon; set => icon = value; }

    /// <summary>
    /// The items count on the slot
    /// </summary>
    public int Count
    {
        get { return Items.Count; }
    }

    /// <summary>
    /// The stack size text
    /// </summary>
    public Text StackSize { get { return stackSize; } }

    public ObservableStack<Item> Items { get => items; }

    private void Awake()
    {
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    /// <summary>
    /// Adds an item to the slot
    /// </summary>
    /// <param name="item">the item to add</param>
    /// <returns>returns true if the item was added</returns>
    public bool AddItem(Item item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.Slot = this;
        return true;
    }

    /// <summary>
    /// Adds a stack of items to the slot
    /// </summary>
    /// <param name="newItems">Stack to add</param>
    /// <returns></returns>
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == Item.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsFull) return false;
                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes the item from the slot
    /// </summary>
    /// <param name="item">item to remove</param>
    public void RemoveItem(Item item)
    {
        if (!IsEmpty) InventoryFrameScrit.Instance.OnItemCountChanged(Items.Pop());
    }

    public void Clear()
    {
        int initCount = Items.Count;
        if (initCount > 0)
            for (int i = 0; i < initCount; i++) InventoryFrameScrit.Instance.OnItemCountChanged(Items.Pop());
    }

    /// <summary>
    /// When the slot is clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryFrameScrit.Instance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.Instance.Moveable != null)
                {
                    if (HandScript.Instance.Moveable is Bag)
                    {
                        if (Item is Bag) InventoryFrameScrit.Instance.SwapBags(HandScript.Instance.Moveable as Bag, Item as Bag);
                    }
                    else if (HandScript.Instance.Moveable is Armory)
                    {
                        if (Item is Armory && (Item as Armory).ArmorType == (HandScript.Instance.Moveable as Armory).ArmorType)
                        {
                            (Item as Armory).Equip();
                            HandScript.Instance.Drop();
                        }
                    }
                }
                else
                {
                    HandScript.Instance.TakeMoveable(Item as IMoveable);
                    InventoryFrameScrit.Instance.FromSlot = this;
                }
            }
            else if (InventoryFrameScrit.Instance.FromSlot == null && IsEmpty)
            {
                if (HandScript.Instance.Moveable is Bag) 
                {
                    Bag bag = (Bag)HandScript.Instance.Moveable;
                    if (bag.InventoryScript != Inventory && InventoryFrameScrit.Instance.EmptySlotCount - bag.SlotCount > 0)
                    {
                        AddItem(bag);
                        bag.BagButton.RemoveBag();
                        HandScript.Instance.Drop();
                    }
                }
                else if (HandScript.Instance.Moveable is Armory)
                {
                    Armory armor = (Armory)HandScript.Instance.Moveable;
                    CharacterPanel.Instance.SelectedButton.DequipArmor();
                    AddItem(armor);
                    HandScript.Instance.Drop();
                }
            }
            else if (InventoryFrameScrit.Instance.FromSlot != null)
            {
                if (PutItemBack() || MergeItems(InventoryFrameScrit.Instance.FromSlot) || SwapItems(InventoryFrameScrit.Instance.FromSlot) || AddItems(InventoryFrameScrit.Instance.FromSlot.Items))
                {
                    HandScript.Instance.Drop();
                    InventoryFrameScrit.Instance.FromSlot = null; // reset
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.Moveable == null) UseItem();
    }

    /// <summary>
    /// Uses the item if it is useable
    /// </summary>
    public void UseItem()
    {
        if (Item is IUseable) (Item as IUseable).Use();
        else if (Item is Armory) (Item as Armory).Equip();
    }

    /// <summary>
    /// Stacks two items
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        if(!IsEmpty && item.name==Item.name && Items.Count < Item.StackSize)
        {
            Items.Push(item);
            item.Slot = this;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Puts the item back in the inventory
    /// </summary>
    /// <returns></returns>
    private bool PutItemBack()
    {
        if (InventoryFrameScrit.Instance.FromSlot == this)
        {
            InventoryFrameScrit.Instance.FromSlot.Icon.color = Color.white;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Swaps two items in the inventory
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty) return false;
        if (from.Item.GetType() != Item.GetType() || from.Count + Count > Item.StackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.Items);
            from.Items.Clear();
            from.AddItems(Items);
            Items.Clear();
            AddItems(tmpFrom);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Merges two identical stacks of items
    /// </summary>
    /// <param name="from">Slot to merge from</param>
    /// <returns></returns>
    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty) return false;
        if (from.Item.GetType() == Item.GetType() && !IsFull)
        {
            // How many free slots do we have in the stack
            int free = Item.StackSize - Count;
            for (int i = 0; i < free; i++)
            {
                AddItem(from.Items.Pop());
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the slot
    /// </summary>
    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty) UIManager.Instance.ShowTooltip(new Vector2(1, 0), transform.position, Item);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
