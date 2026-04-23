using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Transform player;
    protected Enemy_Slime enemy;
    public SlimeGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemyBase as Enemy_Slime;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance != null ? PlayerManager.instance.player?.transform : null;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
            return;

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
