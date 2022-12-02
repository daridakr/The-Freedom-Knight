using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/ManaPotion", order = 1)]
public class ManaPotion : Item, IUseable
{
    [SerializeField]
    private int mana;

    public void Use()
    {
        if (Player.Instance.Mana.CurrentValue < Player.Instance.Mana.MaxValue)
        {
            Remove();
            Player.Instance.Mana.CurrentValue += mana;
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n <color=green>Использование: Восполнение {0} ед. маны</color>", mana);
    }
}