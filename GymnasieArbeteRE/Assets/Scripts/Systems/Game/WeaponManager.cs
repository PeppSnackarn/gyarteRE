using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
   public static WeaponManager Instance;
   [SerializeField] private List<WeaponBase> currentWeapons = new List<WeaponBase>();
   public int currentEquippedWeaponIndex = 0;
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

   private void Start()
   {
      GameManager.Instance.playerInput.Player.SwitchWeapon.performed += ChangeWeapon;
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

   private void ChangeWeapon(InputAction.CallbackContext ctx)
   {
      currentWeapons[currentEquippedWeaponIndex].SetActiveState(false);
      if (currentEquippedWeaponIndex < currentWeapons.Count - 1)
      {
         currentEquippedWeaponIndex++;
      }
      else
      {
         currentEquippedWeaponIndex = 0;
      }
      currentWeapons[currentEquippedWeaponIndex].SetActiveState(true);
   }
}
