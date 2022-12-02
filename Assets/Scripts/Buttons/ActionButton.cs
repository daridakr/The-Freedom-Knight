using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image icon;

    /// <summary>
    /// A reference to the actual button that this button uses
    /// </summary>
    public Button Button { get; private set; }

    public Image Icon { get => icon; set => icon = value; }

    /// <summary>
    /// A reference to the useable on the action button
    /// </summary>
    public IUseable Useable { get; set; }

    public int Count { get { return count; } }

    public Text StackSize { get { return stackSize; } }

    public Stack<IUseable> Useables
    {
        get { return useables; }
        set
        {
            if (value.Count > 0) Useable = value.Peek();
            else Useable = null;
            useables = value;
        }
    }

    [SerializeField]
    private Text stackSize;

    private Stack<IUseable> useables = new Stack<IUseable>();

    private int count;

    void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        InventoryFrameScrit.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    /// <summary>
    /// This is executed the button is clicked
    /// </summary>
    public void OnClick()
    {
        if (HandScript.Instance.Moveable == null)
        {
            if (Useable != null) Useable.Use();
            else if (Useables != null && Useables.Count > 0) Useables.Peek().Use();
        }
    }

    /// <summary>
    /// Checks if someone clicked on the actionbutton
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Moveable != null && HandScript.Instance.Moveable is IUseable) SetUseable(HandScript.Instance.Moveable as IUseable);
        }
    }

    /// <summary>
    /// Sets the useable on an action button
    /// </summary>
    /// <param name="useable"></param>
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            Useables = InventoryFrameScrit.Instance.GetUseables(useable);
            if (InventoryFrameScrit.Instance.FromSlot != null)
            {
                InventoryFrameScrit.Instance.FromSlot.Icon.color = Color.white;
                InventoryFrameScrit.Instance.FromSlot = null;
            }
        }
        else 
        {
            Useables.Clear();
            Useable = useable;
        }
        count = Useables.Count;
        UpdateVisual(useable as IMoveable);
        UIManager.Instance.RefreshTooltip(Useable as IDescribable);
    }

    /// <summary>
    /// Updates the visual representation of the actionbutton
    /// </summary>
    public void UpdateVisual(IMoveable moveable)
    {
        if (HandScript.Instance.Moveable != null) HandScript.Instance.Drop();
        Icon.sprite = moveable.Icon;
        Icon.color = Color.white;
        if (count > 1) UIManager.Instance.UpdateStackSize(this);
        else if (Useable is Spell) UIManager.Instance.ClearStackCount(this);
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUseable && Useables.Count > 0)
        {
            if (Useables.Peek().GetType() == item.GetType())
            {
                Useables = InventoryFrameScrit.Instance.GetUseables(item as IUseable);
                count = Useables.Count;
                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;
        if (Useable != null && Useable is IDescribable)
        {
            tmp = (IDescribable)Useable;
        }
        if (tmp != null) UIManager.Instance.ShowTooltip(new Vector2(1,0), transform.position, tmp);
        // if (Useable != null) UIManager.Instance.ShowTooltip(transform.position);
        //else if (useables.Count > 0) UIManager.Instance.ShowTooltip(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
