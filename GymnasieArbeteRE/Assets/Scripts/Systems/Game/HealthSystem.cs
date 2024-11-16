using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   private int currentHealth;

   #region Properties

   public float MaxHealth => maxHealth;
   public float CurrentHealth => currentHealth;

   #endregion

   private void Start()
   {
      currentHealth = maxHealth;
   }

   public void TakeDamage(int dmg)
   {
      currentHealth -= dmg;
   }
   public void AddHealth(int value)
   {
      currentHealth += value;
   }
}
