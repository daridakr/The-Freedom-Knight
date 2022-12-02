using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Item> items;

    [SerializeField]
    private Inventory inventory;

    public List<Item> Items { get => items; set => items = value; }
    public Inventory Inventory { get => inventory; set => inventory = value; }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if (isOpen) StopInteract();
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();
            Inventory.Clear();
            isOpen = false;
            spriteRenderer.sprite = closedSprite;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void AddItems()
    {
        if (Items != null) foreach (Item item in Items) Inventory.AddItem(item);
    }

    public void StoreItems()
    {
        Items = Inventory.GetItems();
    }
}
