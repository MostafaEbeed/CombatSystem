using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private readonly int RollHash = Animator.StringToHash("Dodge");

    private const float CrossFadeDuration = 0.1f;

    private Vector3 DodgeDirection;

    private float duration = 1f;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 dodgeDirection) : base(stateMachine)
    {
        this.DodgeDirection = dodgeDirection;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(RollHash, CrossFadeDuration);

        
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;

        Move(deltaTime);
        FaceDirection(DodgeDirection);

        stateMachine.ForceReceiver.AddForce(DodgeDirection.normalized * 0.6f);
      
        if (duration < 0.7f)
        {
            stateMachine.Targerter.Cancel();
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0);
            ReturnToLocomotion();
        } 
    }

    public override void Exit()
    {
        //Debug.Log("Exit Dodge");
        
    }
}
