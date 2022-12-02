using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<HandScript>();
            return instance;
        }
    }

    /// <summary>
    /// The current moveable
    /// </summary>
    public IMoveable Moveable { get; set; }

    /// <summary>
    /// The icon of the item, that we acre moving around atm
    /// </summary>
    private Image icon;

    /// <summary>
    /// An offset to move the icon away from the mouse
    /// </summary>
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && Instance.Moveable != null)
        {
            DeleteItem();
        }
       
    }

    /// <summary>
    /// Take a moveable in the hand, so that we can move it around
    /// </summary>
    /// <param name="moveable">The moveable to pick up</param>
    public void TakeMoveable(IMoveable moveable)
    {
        Moveable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = Moveable;
        Moveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    public void Drop()
    {
        Moveable = null;
        icon.color = new Color(0, 0, 0, 0);
        InventoryFrameScrit.Instance.FromSlot = null;
    }

    /// <summary>
    /// Deletes an item from the inventory
    /// </summary>
    public void DeleteItem()
    {

        if (Moveable is Item)
        {
            Item item = (Item)Moveable;
            if (item.Slot != null) item.Slot.Clear();
            else if (item.CharButton != null) item.CharButton.DequipArmor();
        }
        Drop();
        InventoryFrameScrit.Instance.FromSlot = null;
    }
}
