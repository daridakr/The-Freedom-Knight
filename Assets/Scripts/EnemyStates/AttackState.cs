using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AttackState : IState
{
    private Enemy parent;

    private float attackCooldown = .1f;

    private float extraRange = .1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        //if (parent.AttackTime >= attackCooldown && !parent.IsAttacking)

        if (parent.AttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.AttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        if (parent.Target != null)
        {
            // we need to check range and attack
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if (distance >= parent.AttackRange + extraRange && !parent.IsAttacking) parent.ChangeState(new FollowState());
            //if (distance >= parent.AttackRange) parent.ChangeState(new FollowState());
        }
        else parent.ChangeState(new IdleState()); // if parent loses a target
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent.Animator.SetTrigger("attack");
        Player.Instance.Health.CurrentValue -= parent.Damage;
        yield return new WaitForSeconds(parent.Animator.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
