using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {

    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.dir.magnitude > 0.1f)//speed increase whether is running or walking
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement.SwitchState(movement.runningState);
            }
            else
            {
                movement.SwitchState(movement.walkingState);
            }
        }
        //means Crouch state mei change krna hai
        if (Input.GetKeyDown(KeyCode.C))
            movement.SwitchState(movement.crouchState);
    }
}
