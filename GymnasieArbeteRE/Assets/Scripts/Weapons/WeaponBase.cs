using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Gun Values")] 
    [SerializeField] private int damage = 10;
    [SerializeField] private int range = 1000;
    [SerializeField] private int maxAmmo = 8;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private float reloadTime = 1;

    [Header("Spread & Error Values")]
    [SerializeField] private float firingError = 0;
    [SerializeField] private int pelletAmount = 8;

    [Header("Force Values")] 
    [SerializeField] private float knockbackForce = 10;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
    
    //Logic
    private int currentAmmo;
    private float timeAtLastShot;
    private float timeAtLastReload;
    private bool isReloading;
    
    //Private variables
    private PlayerData playerData;
    private Camera playerCam;
    private Player_IA playerInput;

    #region Properties

    public bool IsReloading => isReloading;

    #endregion

    private void Start()
    {
        playerData = PlayerData.Instance;
        playerCam = playerData.PlayerMovement.playerCam;
        playerInput = GameManager.Instance.playerInput;
        currentAmmo = maxAmmo;
        
        //Binding input
        playerInput.Player.Shoot.performed += ctx => Shoot();
        playerInput.Player.Reload.performed += ctx => Reload();
    }
    
    void Shoot()
    {
        if (Time.time > timeAtLastShot && !isReloading)
        {
            if (currentAmmo > 0)
            {
                for (int i = 0; i < pelletAmount; i++)
                {
                    Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, range);
                    if (hit.transform)
                    {
                        if (hit.transform.GetComponent<HealthSystem>())
                        {
                            HealthSystem hitHS = hit.transform.GetComponent<HealthSystem>();
                            if (hitHS != playerData.PlayerHealth)
                            {
                                Debug.LogWarning("Damaged");
                                hitHS.TakeDamage(damage);
                            }
                        }
                        //Can handle bulletImpact VFX here
                        Vector3 hitSpot = hit.normal;

                    }
                    playerData.PlayerMovement.rigidbody.AddForce(-playerCam.transform.forward * knockbackForce, forceMode);
                }
                Debug.Log("Shot!");
                currentAmmo--;
                timeAtLastShot = Time.time + fireRate;
            }
        }
    }

    void Reload()
    {
        if (currentAmmo < maxAmmo)
        {
            isReloading = true;
            StartCoroutine(HandleReloadTime());
        }
    }

    IEnumerator HandleReloadTime()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
    }
}
