using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    /// <summary>
    /// A reference to the spell's rigid body
    /// </summary>
    private Rigidbody2D rigidbody2d;

    /// <summary>
    /// The spell's movement speed
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// The spell's target
    /// </summary>
    public Transform Target { get; private set; }

    private Transform source;

    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        Target = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 direction = Target.position - transform.position;
            rigidbody2d.velocity = direction.normalized * speed;
            float angleSpellRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angleSpellRotation, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == Target)
        {
            Character character = collision.GetComponentInParent<Character>();
            speed = 0;
            character.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            rigidbody2d.velocity = Vector2.zero;
            Target = null;
        }
    }
}
