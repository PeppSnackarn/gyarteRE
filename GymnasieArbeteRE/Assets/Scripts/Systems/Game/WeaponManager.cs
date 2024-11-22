using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
   public static WeaponManager Instance;
   [SerializeField] private List<WeaponBase> currentWeapons = new List<WeaponBase>();
   public int currentEquippedWeaponIndex;
   private static event Action<GameObject> onListUpd;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         onListUpd += InstantiateWeapon;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void AddWeaponToList(GameObject weapon)
   {
      WeaponBase wpn = weapon.GetComponent<WeaponBase>();
      currentWeapons.Add(wpn);
      onListUpd.Invoke(weapon);
   }
   public void ClearList()
   {
      currentWeapons.Clear();
   }
   public void InstantiateWeapon(GameObject weapon)
   {
      GameObject obj = Instantiate(weapon, PlayerData.Instance.HandPos, weapon.transform.rotation); // Wrong rotation
      obj.transform.SetParent(PlayerData.Instance.Hand);
      
   }
}
