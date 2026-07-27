using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        //Debug.Log("Enter Ads State");
        aim.anim.SetBool("Aiming", true);
        aim.currentFov = aim.adsFov;

    }
    public override void UpdateState(AimStateManager aim)
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ExitState(aim, aim.hipFireState);

        }

    }
    void ExitState(AimStateManager aim, AimBaseState state)
    {
        aim.anim.SetBool("Aiming", false);
        aim.SwitchState(state);
    }
}
