using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EvadeState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }

    public void Update()
    {
        parent.Direction = (parent.StartPosition - parent.transform.position).normalized;
        parent.transform.position = Vector2.MoveTowards
            (parent.transform.position, parent.StartPosition, parent.Speed * Time.deltaTime);
        float distance = Vector2.Distance(parent.StartPosition, parent.transform.position);
        if (distance <= 0) parent.ChangeState(new IdleState());
    }
}
