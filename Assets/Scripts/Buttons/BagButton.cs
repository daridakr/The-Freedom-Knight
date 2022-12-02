using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// A reference to the bag item
    /// </summary>
    private Bag bag;

    /// <summary>
    /// Sprites to indicate if the bag is full or empty
    /// </summary>
    [SerializeField]
    private Sprite full, empty;

    [SerializeField]
    private int bagIndex;

    /// <summary>
    /// A reference to the empty bag image to choose it later if it will be full
    /// </summary>
    [SerializeField]
    private Image content;

    /// <summary>
    /// A property for accessing the specific bag
    /// </summary>
    public Bag Bag
    {
        get { return bag; }
        set
        {
            if (value != null)
            {
                content.sprite = full;
                content.color = new Color(255, 255, 255, 255);
                content.transform.localScale = new Vector3(1, 1, 1);
            }
            bag = value;
        }
    }

    public int BagIndex { get => bagIndex; set => bagIndex = value; }

    /// <summary>
    /// If we click the specific bag button
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryFrameScrit.Instance.FromSlot != null && HandScript.Instance.Moveable != null && HandScript.Instance.Moveable is Bag)
            {
                if (Bag != null) InventoryFrameScrit.Instance.SwapBags(Bag, HandScript.Instance.Moveable as Bag);
                else
                {
                    Bag tmp = (Bag)HandScript.Instance.Moveable;
                    tmp.BagButton = this;
                    tmp.Use();
                    Bag = tmp;
                    HandScript.Instance.Drop();
                    InventoryFrameScrit.Instance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift)) HandScript.Instance.TakeMoveable(Bag);
            else if (bag != null) { bag.InventoryScript.OpenClose(); InventoryFrameScrit.Instance.OpenClose(bag); }
        }
    }

    /// <summary>
    /// Removes the bag from the bagbar
    /// </summary>
    public void RemoveBag()
    {
        InventoryFrameScrit.Instance.RemoveBag(Bag);
        Bag.BagButton = null;
        foreach (Item item in Bag.InventoryScript.GetItems()) InventoryFrameScrit.Instance.AddItem(item);
        Bag = null;
        content.sprite = empty;
        content.color = Color.HSVToRGB(0,0,0);
        content.transform.localScale = new Vector3(0.6f, 0.6f, 1);
    }
}
