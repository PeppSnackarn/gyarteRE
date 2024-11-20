using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    private PlayerMovement playerMovement;
    private HealthSystem playerHealth;
    private WeaponManager weaponManager;

    #region Properties

    public PlayerMovement PlayerMovement => playerMovement;
    public HealthSystem PlayerHealth => playerHealth;
    public WeaponManager WeaponManager => weaponManager;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<HealthSystem>();
        weaponManager = GetComponentInChildren<WeaponManager>();
    }
}
