using UnityEngine;

public class WalkingState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);

    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ExitState(movement, movement.runningState);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ExitState(movement, movement.crouchState);
        }
        else if (movement.dir.magnitude < 0.1f)
        {
            ExitState(movement, movement.idleState);
        }
        if (movement.vInput > 0)
            movement.currentSpeed = movement.walkSpeed;
        else
            movement.currentSpeed = movement.walkBackSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            // movement.SwitchState(movement.jumpState);
            ExitState(movement, movement.jumpState);

        }
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);

    }
}
