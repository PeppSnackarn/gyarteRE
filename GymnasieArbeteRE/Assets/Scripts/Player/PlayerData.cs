using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    #region Properties

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerHealth PlayerHealth => playerHealth;

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
        playerHealth = GetComponent<PlayerHealth>();
    }
}
