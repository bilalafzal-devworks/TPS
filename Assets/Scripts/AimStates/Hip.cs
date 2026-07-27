using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hip : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        //Debug.Log("Enter hip State");
        aim.currentFov = aim.hipFov;

    }
    public override void UpdateState(AimStateManager aim)
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            aim.SwitchState(aim.adsState);
        }
    }
}
