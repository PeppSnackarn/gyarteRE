using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    private PlayerMovement playerMovement;
    private HealthSystem playerHealth;
    [SerializeField] private Transform hand; 

    #region Properties

    public PlayerMovement PlayerMovement => playerMovement;
    public HealthSystem PlayerHealth => playerHealth;
    public Vector3 HandPos => hand.position;
    public Transform Hand => hand;

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
    private void OnDestroy()
    {
        Instance = null;
        WeaponManager.Instance.ClearList();
    }
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<HealthSystem>();
    }
}
