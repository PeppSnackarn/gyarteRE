using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health settings")] 
    [SerializeField] private float maxHealth = 100f;

    [Header("Attributes")] 
    [SerializeField] private float currentHealth;

    #region Properties

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
