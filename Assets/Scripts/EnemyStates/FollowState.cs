using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The enemy's follow state
/// </summary>
class FollowState : IState
{
    /// <summary>
    /// A reference to the parent
    /// </summary>
    private Enemy parent;

    /// <summary>
    /// This is called whenever we enter the state
    /// </summary>
    /// <param name="parent">The parent enemy</param>
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// This is called whenever we exit the state
    /// </summary>
    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    /// <summary>
    /// This is called as long as we are inside the state
    /// </summary>
    public void Update()
    {
        // as long as we have a target, then we need to keep moving
        if (parent.Target != null)
        {
            // find the target's direction
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;
            // moves the enemy towards the target
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);
            // distance between target and parent
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if (distance <= parent.AttackRange) parent.ChangeState(new AttackState());
        }
        if (!parent.InRange) parent.ChangeState(new EvadeState()); //else we need to go back to idle
    }
}
