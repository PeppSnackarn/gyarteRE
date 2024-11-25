using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
    [SerializeField] private bool isActive = false;
    
    //Private variables
    private PlayerData playerData;
    private Camera playerCam;
    private Player_IA playerInput;

    #region Properties

    public bool IsReloading => isReloading;

    #endregion
    
    protected virtual void Start()
    {
        playerData = PlayerData.Instance;
        playerCam = playerData.PlayerMovement.playerCam;
        playerInput = GameManager.Instance.playerInput;
        currentAmmo = maxAmmo;
        
        //Binding input
        playerInput.Player.Shoot.performed += Shoot;
        playerInput.Player.AltShoot.performed += AltShoot;
        playerInput.Player.Reload.performed += Reload;
    }

    private void OnDestroy()
    {
        playerInput.Player.Shoot.performed -= Shoot;
        playerInput.Player.AltShoot.performed -= AltShoot;
        playerInput.Player.Reload.performed -= Reload;
    }
    
    public void SetActiveState(bool state)
    {
        isActive = state;
        gameObject.SetActive(state);
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        if (Time.time > timeAtLastShot && !isReloading)
        {
            if (currentAmmo > 0)
            {
                for (int i = 0; i < pelletAmount; i++)
                {
                    Physics.Raycast(playerCam.transform.position, GetShootingDirection(), out RaycastHit hit, range);
                    if (hit.transform)
                    {
                        Debug.Log(hit.transform.name);
                        if (hit.transform.GetComponent<HealthSystem>())
                        {
                            HealthSystem hitHS = hit.transform.GetComponent<HealthSystem>();
                            if (hitHS != playerData.PlayerHealth)
                            {
                                hitHS.TakeDamage(damage);
                            }
                        }
                        //Can handle bulletImpact VFX here
                        Vector3 hitSpot = hit.normal;

                    }
                    playerData.PlayerMovement.rigidbody.AddForce(-playerCam.transform.forward * knockbackForce, forceMode);
                }
                currentAmmo--;
                timeAtLastShot = Time.time + fireRate;
            }
        }
    }

    protected virtual void AltShoot(InputAction.CallbackContext ctx)
    {
        
    }

    Vector3 GetShootingDirection()
    {
        Vector3 targetPos = playerCam.transform.position + playerCam.transform.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-firingError, firingError),
            targetPos.y + Random.Range(-firingError, firingError),
            targetPos.z + Random.Range(-firingError, firingError));
        Vector3 direction = targetPos - playerCam.transform.position;
        return direction.normalized;
    }

    void Reload(InputAction.CallbackContext ctx)
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
