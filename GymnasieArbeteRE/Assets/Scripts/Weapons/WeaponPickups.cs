using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickups : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;

    private void OnValidate()
    {
        if (weaponPrefab != null && weaponPrefab.GetComponent<WeaponBase>() == null)
        {
            Debug.LogError("The assigned prefab: "+weaponPrefab.name+" Does not derive from WeaponBase");
            weaponPrefab = null;
        }  
    }

    private void Start()
    {
        if (weaponPrefab == null)
        {
            Debug.LogError(gameObject.name+" does not have an assigned value and will be destroyed");
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerData>())
        {
           WeaponManager.Instance.InstantiateWeapon(weaponPrefab);
           Destroy(gameObject);
        }
    }
}
