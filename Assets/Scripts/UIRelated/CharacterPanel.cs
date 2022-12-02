using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    public static CharacterPanel Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<CharacterPanel>();
            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton head, chest, belt, hands, feet, main, off, rightFinger, leftFinger, neck;

    public CharButton SelectedButton { get; set; }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armory armor)
    {
        switch(armor.ArmorType)
        {
            case ArmorType.Head:
                head.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Belt:
                belt.EquipArmor(armor);
                break;
            case ArmorType.Hands:
                hands.EquipArmor(armor);
                break;
            case ArmorType.Feet:
                feet.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                main.EquipArmor(armor);
                break;
            case ArmorType.OffHand:
                off.EquipArmor(armor);
                break;
            //case ArmorType.TwoHand:
            //    break;
            case ArmorType.RightFinger:
                rightFinger.EquipArmor(armor);
                break;
            case ArmorType.LeftFinger:
                leftFinger.EquipArmor(armor);
                break;
            case ArmorType.Neck:
                neck.EquipArmor(armor);
                break;
        }
    }
}
