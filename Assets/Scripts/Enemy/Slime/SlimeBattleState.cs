using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    protected Enemy_Slime enemy;
    private Transform player;
    private int moveDir;
    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
            player = PlayerManager.instance.player.transform;

        if (player != null && player.GetComponent<PlayerStats>() != null && player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (enemy.IsPlayerDetected())
        {

            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState);
        }


        if (player.position.x > enemy.transform.position.x)

            moveDir = 1;

        else if (player.position.x < enemy.transform.position.x)

            moveDir = -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCoolown, enemy.maxAttackCoolown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
