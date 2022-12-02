using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is an abstract class that all characters needs to inherit from
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// The Character's movement speed
    /// </summary>
    [SerializeField]
    private float speed;

    [SerializeField]
    private string type;

    /// <summary>
    /// The Character's direction
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// A reference to the character's animator
    /// </summary>
    public Animator Animator { get; set; }

    /// <summary>
    /// The character's rigibody
    /// </summary>
    private Rigidbody2D rigidbody2d;

    /// <summary>
    /// Indicates if we are mooving or not
    /// </summary>
    public bool IsMoving
    {
        get { return Direction.x != 0 || Direction.y != 0; }
        set { }
    }
   
    /// <summary>
    /// Property for set and get character's health
    /// </summary>
    public Stat Health { get => health; set => health = value; } 

    /// <summary>
    /// The character's target
    /// </summary>
    public Transform Target { get; set; }

    /// <summary>
    /// Property for set and get character's direction
    /// </summary>
    public Vector2 Direction { get => direction; set => direction = value; }

    /// <summary>
    /// Property for set and get character's speed
    /// </summary>
    public float Speed { get => speed; set => speed = value; }
   
    /// <summary>
    /// Indicates if character alive or not
    /// </summary>
    public bool IsAlive
    {
        get { return health.CurrentValue > 0; }
        set { }
    }

    /// <summary>
    /// Indicates if the character is attacking or not
    /// </summary>
    public bool IsAttacking { get; set; }
    public string CharacterName { get => characterName; }
    public int Level { get => level; set => level = value; }
    public string Type { get => type; }
    //public Image Portrait { get => portrait; set => portrait = value; }

    /// <summary>
    /// A reference to the attack coroutine
    /// </summary>
    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    private string currentLocation;

    [SerializeField]
    private int level;

    /// <summary>
    /// The character's health
    /// </summary>
    [SerializeField]
    protected Stat health;

    /// <summary>
    /// The character's current health
    /// </summary>
    [SerializeField]
    private float healthValue;
    
    [SerializeField]
    private string characterName;

    //[SerializeField]
    //private Image portrait;

    protected virtual void Start()
    {
        Health.Initialize(healthValue, healthValue);
        Animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleAnimationLayers();
        Flip();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves the character
    /// </summary>
    public void Move()
    {
        if(IsAlive) rigidbody2d.velocity = Direction.normalized * Speed;  
    }

    /// <summary>
    /// Makes sure that the right animation layer is playing
    /// </summary>
    public virtual void HandleAnimationLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateAnimationLayer("RunLayer");
                Animator.SetFloat("x", Direction.x);
                Animator.SetFloat("y", Direction.y);
            }
            else if (IsAttacking)
            {
                ActivateAnimationLayer("AttackLayer");
            }
            else ActivateAnimationLayer("IdleLayer");
        }
        else ActivateAnimationLayer("DeathLayer");
    }
   
    public void Flip()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary>
    /// Activates an animation layer based on a string
    /// </summary>
    public virtual void ActivateAnimationLayer(string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }
        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }
    
    /// <summary>
    /// Makes the character take damage
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageSource"></param>
    public virtual void TakeDamage(float damage, Transform damageSource)
    {
        Health.CurrentValue -= damage;
        if (Health.CurrentValue <= 0)
        {
            Direction = Vector2.zero;
            rigidbody2d.velocity = Direction;
            GameManager.Instance.OnKillConfirmed(this);
            Animator.SetTrigger("die");
            if (this is Enemy) Player.Instance.GainXp(XPManager.CalculateXP((this as Enemy)));
        }
    }
}
