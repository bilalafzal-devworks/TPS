using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public int clipSize = 30; //can be change if a weapon is different
    public int extraAmmo = 120;
    public int currentAmmo;
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.R)) Reload();
    // }

    public void Reload()
    {
        if (extraAmmo >= clipSize)
        {
            int ammoToReload = clipSize - currentAmmo;
            extraAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
        }
        else if (extraAmmo > 0)
        {
            if (currentAmmo + extraAmmo > clipSize)
            {
                int leftOverAmmo = currentAmmo + extraAmmo - clipSize;
                currentAmmo = clipSize;
                extraAmmo = leftOverAmmo;

            }
            else
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }
}
