using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;

    private bool alreadyAppliedForce = false;

    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.KnockBack);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);

        if (stateMachine.Targerter.GetTarget() != null)
            FaceDirection(stateMachine.Targerter.GetTarget().transform.position);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator);

        

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if(normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if(stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            stateMachine.Targerter.Cancel();
            if (stateMachine.Targerter.CurrentTarget != null)
            {
                //stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }


    private void TryComboAttack(float normalizedTime)
    {
        if(attack.ComboStateIndex == -1) { return; }

        if(normalizedTime < attack.ComboAttackTime) { return; }

        Debug.Log("ddddddd" + attack.ComboStateIndex);
        if(stateMachine.Targerter.GetTarget() != null)
            FaceDirection(stateMachine.Targerter.GetTarget().transform.position);

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            ( 
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyAppliedForce = true;
    }
}
