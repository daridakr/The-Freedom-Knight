using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Superclass for all items
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    /// <summary>
    /// Icon used when moving and placing the items
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// The size of the stack, less than 2 is not stackable
    /// </summary>
    [SerializeField]
    private int stackSize;

    /// <summary>
    /// The item's title
    /// </summary>
    [SerializeField]
    private string title;

    /// <summary>
    /// The item's quality
    /// </summary>
    [SerializeField]
    private Quality quality;

    /// <summary>
    /// A refernce to the slot that this item is sitting on
    /// </summary>
    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    private int cost;

    /// <summary>
    /// Property for accessing the icon
    /// </summary>
    public Sprite Icon { get => icon; }

    /// <summary>
    /// Property for accessing the stackable
    /// </summary>
    public int StackSize { get => stackSize; }

    /// <summary>
    /// Property for accessing the slotScript
    /// </summary>
    public SlotScript Slot { get => slot; set => slot = value; }
    public Quality Quality { get => quality; }
    public string Title { get => title; }
    public CharButton CharButton
    {
        get { return charButton; }
        set
        {
            Slot = null;
            charButton = value;
        }
    }

    public int Cost { get => cost; set => cost = value; }

    /// <summary>
    /// Returns a description of this specific item
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", QualityColor.Colors[Quality], Title);
    }

    /// <summary>
    /// Removes the item from the inventory
    /// </summary>
    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItem(this);
            
        }
    }    
    
}
