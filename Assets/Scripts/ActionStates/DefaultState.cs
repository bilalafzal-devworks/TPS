using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public override void EnterState(ActionStateManager state)
    {
        state.lHandIK.weight = 1;
        state.rHandAim.weight = 1;

    }

    public override void UpdateState(ActionStateManager state)
    {
        state.rHandAim.weight = Mathf.Lerp(state.rHandAim.weight, 1, 10 * Time.deltaTime);
        state.lHandIK.weight = Mathf.Lerp(state.lHandIK.weight, 1, 10 * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.R) && CanReload(state))
            state.SwitchState(state.reloadState);

    }
    bool CanReload(ActionStateManager state)
    {
        if (state.ammo.currentAmmo == state.ammo.clipSize) return false;
        else if (state.ammo.extraAmmo == 0) return false;
        else return true;
    }
}
