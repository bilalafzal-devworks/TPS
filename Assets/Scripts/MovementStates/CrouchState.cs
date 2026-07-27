using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Crouching", true);

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) ExitState(movement, movement.runningState);

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (movement.dir.magnitude < 0.1f)
                ExitState(movement, movement.idleState);
            else
                ExitState(movement, movement.walkingState);
        }
        if (movement.vInput > 0)
            movement.currentSpeed = movement.crouchSpeed;
        else
            movement.currentSpeed = movement.crouchBackSpeed;
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}
