using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void CharacterRemoved();
public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private Sprite portrait;

    [SerializeField]
    private int damage;

    public Sprite Portrait { get => portrait; }

    /// <summary>
    /// A canvasgroup for the healthbar
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup; 

    /// <summary>
    /// The enemes current state 
    /// </summary>
    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    /// <summary>
    /// The enemes attack range
    /// </summary>
    public float AttackRange { get; set; }

    [SerializeField]
    private float initialAggroRange;

    public float AggroRange { get; set; }

    public bool InRange
    {
        get { return Vector2.Distance(transform.position, Target.position) < AggroRange; }
    }

   
    /// <summary>
    /// How much time has passed since the last attack
    /// </summary>
    public float AttackTime { get; set; }

    public Vector3 StartPosition { get; set; }
    public int Damage { get => damage; set => damage = value; }

    // from the start
    protected void Awake()
    {
        StartPosition = transform.position;
        AggroRange = initialAggroRange;
        AttackRange = .5f;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if(IsAlive)
        {
            if (!IsAttacking) AttackTime += Time.deltaTime;
            currentState.Update();
        }
        base.Update();
    }
   
    /// <summary>
    /// When the enemy is selected
    /// </summary>
    /// <returns></returns>
    public Transform Select()
    {
        healthGroup.alpha = 1;
        return hitBox;
    }
    
    /// <summary>
    /// When player deselect the enemy
    /// </summary>
    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.Instance.HideTargetFrame);
    }

    /// <summary>
    /// Makes the enemy take damage when hit
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage, Transform damageSource)
    {
        if(!(currentState is EvadeState))
        {
            SetTarget(damageSource);
            base.TakeDamage(damage, damageSource);
            OnHealthChanged(Health.CurrentValue);
        }
    }
    
    /// <summary>
    /// Changes the enemes state
    /// </summary>
    /// <param name="newState">The new state</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (Target == null && !(currentState is EvadeState))
        {
            // distance between enemy and target
            float distance = Vector2.Distance(transform.position, target.position);
            AggroRange = initialAggroRange;
            AggroRange += distance;
            Target = target;
        }
    }

    public void Reset()
    {
        Target = null;
        AggroRange = initialAggroRange;
        Health.CurrentValue = Health.MaxValue;
        OnHealthChanged(health.CurrentValue); // for unit frame update
    }

    public void Interact()
    {
        if (!IsAlive) lootTable.ShowLoot(this);
    }

    public void StopInteract()
    {
        LootWindow.Instance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null) healthChanged(health);
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null) characterRemoved();
        Destroy(gameObject);
    }
}
