using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType { Head, Chest, Belt, Hands, Feet, MainHand, OffHand, TwoHand, RightFinger, LeftFinger, Neck }

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armory : Item
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int armor;

    [SerializeField]
    private int damage;

    [SerializeField]
    private int force;

    [SerializeField]
    private int dexterity;

    [SerializeField]
    private int magic;

    [SerializeField]
    private int vitality;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int requiredForce;

    [SerializeField]
    private int requiredDexterity;

    [SerializeField]
    private int requiredMagic;

    [SerializeField]
    private AnimationClip[] animationClips;

    internal ArmorType ArmorType { get => armorType; }
    public AnimationClip[] AnimationClips { get => animationClips; }

    // not realese version
    public override string GetDescription()
    {
        // armortype
        string stats = string.Empty;
        if (armor > 0) stats += string.Format("\n �����: {0}", armor);
        if (damage > 0) stats += string.Format("\n ����: {0}", damage);
        if (force > 0) stats += string.Format("\n <color=green>+{0} � ����</color>", force);
        if (dexterity > 0) stats += string.Format("\n <color=green>+{0} � ��������</color>", dexterity);
        if (magic > 0) stats += string.Format("\n <color=green>+{0} � �����</color>", magic);
        if (vitality > 0) stats += string.Format("\n <color=green>+{0} � ���������</color>", vitality);
        if (requiredForce > 0) stats += string.Format("\n ��������� {0} ����", requiredForce);
        if (requiredDexterity > 0) stats += string.Format("\n ��������� {0} ��������", requiredDexterity);
        if (requiredMagic > 0) stats += string.Format("\n ��������� {0} ������ �����", requiredMagic);
        if (strength > 0) stats += string.Format("\n ���������: {0}/{0}", strength);
        if (Cost < 0) stats += string.Format("\n <color=#E5CC80>���������</color>");
        else stats += string.Format("\n ���������: {0}", Cost);
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.Instance.EquipArmor(this);
    }
}
