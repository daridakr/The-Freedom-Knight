using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/HealthPotion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;

    public void Use()
    {
        if (Player.Instance.Health.CurrentValue < Player.Instance.Health.MaxValue)
        {
            Remove();
            Player.Instance.Health.CurrentValue += health;
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n <color=green>Использование: Восполнение {0} ед. здоровья</color>", health);
    }
}
