using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager state)
    {
        state.lHandIK.weight = 0;
        state.rHandAim.weight = 0;
        state.anim.SetTrigger("Reload");
    }

    public override void UpdateState(ActionStateManager state)
    {

    }
}
