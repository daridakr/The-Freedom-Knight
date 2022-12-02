using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable, IDescribable
{
    /// <summary>
    /// The Spell's name
    /// </summary>
    [SerializeField]
    private string name;

    /// <summary>
    /// The Spell's damage
    /// </summary>
    [SerializeField]
    private int damage;

    /// <summary>
    /// The Spell's manaCost
    /// </summary>
    [SerializeField]
    private int manaCost;

    /// <summary>
    /// The Spell's icon
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// The Spell's speed
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// The Spell's castTime
    /// </summary>
    [SerializeField]
    private float castTime;

    /// <summary>
    /// The Spell's prefab
    /// </summary>
    [SerializeField]
    private GameObject spellPrefab;

    /// <summary>
    /// The Spell's color
    /// </summary>
    [SerializeField]
    private Color barColor;

    /// <summary>
    /// Property for reading the spell's name
    /// </summary>
    public string Name { get => name; }

    /// <summary>
    /// Property for reading the damage
    /// </summary>
    public int Damage { get => damage; }

    /// <summary>
    /// Property for reading the icon
    /// </summary>
    public Sprite Icon { get => icon; }

    /// <summary>
    /// Property for reading the spell's speed
    /// </summary>
    public float Speed { get => speed; }

    /// <summary>
    /// Property for reading the cast time
    /// </summary>
    public float CastTime { get => castTime; }

    /// <summary>
    /// Property for reading the spell's prefab
    /// </summary>
    public GameObject SpellPrefab { get => spellPrefab; }

    /// <summary>
    /// Property for reading the spell's color
    /// </summary>
    public Color BarColor { get => barColor; }
    public int ManaCost { get => manaCost; set => manaCost = value; }

    public string GetDescription()
    {
        return string.Format("{0}\n Применение: {1} сек\n Урон: {2}\n Мана: {3}", name, castTime, damage, ManaCost);
    }

    public void Use()
    {
        Player.Instance.CastSpell(Name);
    }
}