using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armory equippedArmor;

    [SerializeField]
    private Image icon;

    [SerializeField]
    public GearSocket gearSocket;

    public Armory EquippedArmor { get => equippedArmor; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.Instance.Moveable is Armory)
            {
                Armory tmp = (Armory)HandScript.Instance.Moveable;
                if (tmp.ArmorType == armorType) EquipArmor(tmp);
                UIManager.Instance.RefreshTooltip(tmp);
            }
            else if (HandScript.Instance.Moveable == null && EquippedArmor != null)
            {
                HandScript.Instance.TakeMoveable(EquippedArmor);
                CharacterPanel.Instance.SelectedButton = this;
                icon.color = Color.grey;
            }

        }
    }

    public void EquipArmor(Armory armor)
    {
        armor.Remove();
        if (EquippedArmor != null)
        {
            if (EquippedArmor != armor) armor.Slot.AddItem(EquippedArmor);
            UIManager.Instance.RefreshTooltip(EquippedArmor);
        }
        else UIManager.Instance.HideTooltip();
        icon.enabled = true;
        icon.sprite = armor.Icon;
        icon.color = Color.white;
        equippedArmor = armor;
        equippedArmor.CharButton = this;
        if (HandScript.Instance.Moveable == (armor as IMoveable)) HandScript.Instance.Drop();
        if (gearSocket != null && EquippedArmor.AnimationClips != null) gearSocket.Equip(EquippedArmor.AnimationClips);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EquippedArmor != null) UIManager.Instance.ShowTooltip(new Vector2(0, 1.4f), transform.position, EquippedArmor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;
        if (gearSocket != null && EquippedArmor.AnimationClips != null) gearSocket.Dequip();
        equippedArmor.CharButton = null;
        equippedArmor = null;
    }
}
