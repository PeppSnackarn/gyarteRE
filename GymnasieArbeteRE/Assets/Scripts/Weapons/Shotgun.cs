using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : WeaponBase
{
    protected override void AltShoot(InputAction.CallbackContext ctx)
    {
        base.AltShoot(ctx);
        Debug.Log("Registered input");
    }
}
