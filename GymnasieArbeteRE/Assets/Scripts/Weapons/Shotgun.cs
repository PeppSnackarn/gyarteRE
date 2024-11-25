using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : WeaponBase
{
    protected override void AltShoot(InputAction.CallbackContext ctx)
    {
        base.AltShoot(ctx); // Fire a grenade ("an unshot cartridge") will function like a splinter grenade
        Debug.Log("Registered input");
    }
}
