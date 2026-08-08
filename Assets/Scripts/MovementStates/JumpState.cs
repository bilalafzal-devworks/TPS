using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        if (movement.previousState == movement.idleState) movement.anim.SetTrigger("IdleJump");
        else if (movement.previousState == movement.walkingState || movement.previousState == movement.runningState)
            movement.anim.SetTrigger("RunJump");

    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.isJumped && movement.isGrounded())
        {
            movement.isJumped = false;
            //movement.anim.SetBool("Failing")
            if (movement.hzInput == 0 && movement.vInput == 0) movement.SwitchState(movement.idleState);
            else if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.runningState);
            else movement.SwitchState(movement.walkingState);
        }

    }
}
