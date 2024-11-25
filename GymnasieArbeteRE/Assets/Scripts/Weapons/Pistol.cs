using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : WeaponBase
{
    protected override void AltShoot(InputAction.CallbackContext ctx)
    {
        base.AltShoot(ctx);
        Debug.Log("Input registered"); // Start spinning all revolver barrels and then "fan the hammer" (accurate)
    }
}
