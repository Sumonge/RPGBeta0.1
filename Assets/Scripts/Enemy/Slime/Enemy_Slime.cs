using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }


    #endregion


    protected override void Awake()
    {
        base.Awake();

        SetupFailFacingDir(-1);

        battleTime = 5f;
        attackDistance = 1.5f;
        attackCooldown = 1f;
        minAttackCoolown = 1f;
        maxAttackCoolown = 2f;
        stunDuration = 3f;
        stunDirection = new Vector2(3f, 6f);

        idleState = new SlimeIdleState(this, stateMachine, "Idle",this);
        moveState = new SlimeMoveState(this, stateMachine, "Move",this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack",this);
        battleState = new SlimeBattleState(this, stateMachine, "Move",this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stun", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);

    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;

    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

    }
}
