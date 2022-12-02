 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    /// <summary>
    /// The amount of slots this bag has
    /// </summary>
    [SerializeField]
    private int slotCount;

    /// <summary>
    /// A reference to a bag prefab, so that we can initialize a bag in the game
    /// </summary>
    [SerializeField]
    protected GameObject bagPrefab;

    /// <summary>
    /// A reference to the inventoryScript, that this bag belongs to
    /// </summary>
    public Inventory InventoryScript { get; set; }

    /// <summary>
    /// A reference to the bag button this bag is attached to
    /// </summary>
    public BagButton BagButton { get; set; }

    /// <summary>
    /// Property for getting the slots
    /// </summary>
    public int SlotCount { get => slotCount; }

    /// <summary>
    /// Initializes the bag with an amount of slots
    /// </summary>
    /// <param name="slots"></param>
    public void Initialize(int slots)
    {
        this.slotCount = slots;
    }

    /// <summary>
    /// Equipts the bag
    /// </summary>
    public void Use()
    {
        if (InventoryFrameScrit.Instance.CanAddBag)
        {
            Remove();
            InventoryFrameScrit.Instance.OpenClose(this);
            InventoryScript = Instantiate(bagPrefab, InventoryFrameScrit.Instance.transform).GetComponent<Inventory>();
            InventoryScript.AddSlots(SlotCount);
            if (BagButton == null) InventoryFrameScrit.Instance.AddBag(this);
            else InventoryFrameScrit.Instance.AddBag(this, BagButton);
            InventoryScript.Index = BagButton.BagIndex;
        }
    }

    public void SetUpScript()
    {
        InventoryScript = Instantiate(bagPrefab, InventoryFrameScrit.Instance.transform).GetComponent<Inventory>();
        InventoryScript.AddSlots(slotCount);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n —умка (€чеек: {0})", slotCount);
    }
}
