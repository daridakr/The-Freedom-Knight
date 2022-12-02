using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains functionality that is specific to the Player
/// </summary>
public class Player : Character
{
    private static Player instance; 

    public static Player Instance
    {
        // only one instance in game
        get
        {
            if (instance == null) instance = FindObjectOfType<Player>();
            return instance;
        }
    }

    //indicators

    /// <summary>
    /// The player's maximum health
    /// </summary>
    //private float maxHealth = 100;
   
    /// <summary>
    /// The player's mana
    /// </summary>
    [SerializeField]
    private Stat mana;

    [SerializeField]
    private Stat xpStat;

    /// <summary>
    /// The player's current mana
    /// </summary>
    private float manaValue = 100;

    /// <summary>
    /// The player's maximum mana
    /// </summary>
    private float maxMana = 100;

    private Vector3 min, max;

    //spells

    /// <summary>
    /// Exit points for the spells
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    [SerializeField]
    private Animator ding;
    
    /// <summary>
    /// Index that keeps track of which exit point to use, 2 is default down
    /// </summary>
    private int exitIndex = 2;

    private IInteractable interactable;

    /// <summary>
    /// An array of blocks used for blocking the player's sight
    /// </summary>
    [SerializeField]
    private Block[] sightBlocks;

    /// <summary>
    /// Layer mask number code with sight blocks
    /// </summary>
    private int blockLayerMaskCode = 256;

    [SerializeField]
    private GearSocket[] gearSockets;

    public int Money { get; set; }
    public IInteractable Interactable { get => interactable; set => interactable = value; }
    public Stat XpStat { get => xpStat; set => xpStat = value; }
    public Stat Mana { get => mana; set => mana = value; }

    protected override void Start()
    {
        Money = 130;
        Mana.Initialize(manaValue, maxMana);
        XpStat.Initialize(0, Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f)));
        base.Start();
    }

    protected override void Update()
    {
        Mana.CurrentValue += 0.03f;
        Health.CurrentValue += 0.01f;
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
    }
    
    /// <summary>
    /// Listen's to the player input
    /// </summary>
    private void GetInput()
    {
        Direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.X)) GainXp(16);
        if (IsMoving) StopAttack();
    }

    /// <summary>
    /// Set's the player's limits so that he can't leave the game world
    /// </summary>
    /// <param name="min">The minimum position of the player</param>
    /// <param name="max">The maximum position of the player</param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
   
    /// <summary>
    /// A coroutine for attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack(string spellName)
    {
        if (Mana.CurrentValue > 0 )
        {
            Transform currentTarget = Target;
            Spell newSpell = SpellBook.Instance.CastSpell(spellName);
            IsAttacking = true;
            Animator.SetBool("attack", IsAttacking);
            foreach (GearSocket gear in gearSockets) gear.Animator.SetBool("attack", IsAttacking);
            yield return new WaitForSeconds(newSpell.CastTime);
            if (currentTarget != null && InLineOfSight())
            {
                SpellScript spell = Instantiate(newSpell.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
                spell.Initialize(currentTarget, newSpell.Damage, transform);
            }
            Mana.CurrentValue -= newSpell.ManaCost;
            StopAttack();
        }
    }
    
    /// <summary>
    /// Casts a spell
    /// </summary>
    public void CastSpell(string spellName)
    {
        UpdateBlocks();
        if (Target != null && Target.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight()) attackRoutine = StartCoroutine(Attack(spellName));
    }
    
    /// <summary>
    /// Checks if the target is in line of sight
    /// </summary>
    /// <returns></returns>
    private bool InLineOfSight()
    {
        if (Target != null) 
        {
            // calculates the target's direction
            Vector3 targetDirection = (Target.transform.position - transform.position).normalized;
            // throws a raycast in the direction of the target
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), blockLayerMaskCode);
            // if we dont hot the block, then we can cast a spell
            if (hit.collider == null) return true;
        }
        return false;
    }
    
    /// <summary>
    /// Changes the blocks based on the player's direction
    /// </summary>
    private void UpdateBlocks()
    {
        foreach(Block block in sightBlocks)
        {
            block.Deactivate();
        }
        sightBlocks[exitIndex].Activate();
    }

    /// <summary>
    /// Stops the attack
    /// </summary>
    public void StopAttack()
    {
        SpellBook.Instance.StopCasting();
        IsAttacking = false;
        Animator.SetBool("attack", IsAttacking);
        foreach (GearSocket gear in gearSockets) gear.Animator.SetBool("attack", IsAttacking);
        if (attackRoutine != null) StopCoroutine(attackRoutine);
    }

    public override void HandleAnimationLayers()
    {
        base.HandleAnimationLayers();
        if (IsMoving) foreach (GearSocket gear in gearSockets) gear.SetXAndY(Direction.x, Direction.y);
    }

    public override void ActivateAnimationLayer(string layerName)
    {
        base.ActivateAnimationLayer(layerName);
        foreach (GearSocket gear in gearSockets) gear.ActivateAnimationLayer(layerName);
    }
    
    public void Interact()
    {
        if (Interactable != null) Interactable.Interact();
    }

    public void GainXp(int xp)
    {
        XpStat.CurrentValue += xp;
        if (XpStat.CurrentValue >= XpStat.MaxValue) StartCoroutine(Ding());
    }

    private IEnumerator Ding()
    {
        while (!XpStat.IsFull) yield return null;
        Level++;
        Health.CurrentValue = 100;
        Mana.CurrentValue = 100;
        ding.SetTrigger("Ding");
        UIManager.Instance.PlayerLevel.text = string.Format("Уровень: " + Level.ToString());
        XpStat.MaxValue = 100 * Level * Mathf.Pow(Level, 0.5f);
        XpStat.MaxValue = Mathf.Floor(XpStat.MaxValue);
        XpStat.CurrentValue = XpStat.Overflow;
        XpStat.Reset();
        if (XpStat.CurrentValue >= XpStat.MaxValue) StartCoroutine(Ding());
    }

    public void UpdateLevel()
    {
        UIManager.Instance.PlayerLevel.text = "Уровень: " + Level.ToString();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable") Interactable = collision.GetComponent<IInteractable>();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (Interactable != null)
            {
                Interactable.StopInteract();
                Interactable = null;
            }
        }
    }
}