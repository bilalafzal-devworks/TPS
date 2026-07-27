using UnityEngine;

public class RunningState : MovementBaseState
{

    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Running", true);

    }
    public override void UpdateState(MovementStateManager movement)
    {
        //we make the animator in such way that no directly go back to idle
        //faster(Running) -> Little-faster(Walking)->Slow-Stop(Idle)
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.walkingState);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.idleState);
        else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.crouchState);
        
        //speed Adjustment
        if (movement.vInput > 0)
            movement.currentSpeed = movement.runSpeed;
        else
            movement.currentSpeed = movement.runBackSpeed;
    }
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
