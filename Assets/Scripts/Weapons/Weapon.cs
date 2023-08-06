using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected string weaponName;
    [Header("Ammo")]
    [SerializeField] protected GameObject ammo;
    [SerializeField] public int ammoMax = 10;
    [SerializeField] public GameObject ammoUI;
    [Header("Load, Reload and Recoil")]
    [SerializeField] public float recoilTime = 0.1f;
    [SerializeField] public float reloadTime = 2f;
    [SerializeField] public float loadTime = 0f;
    [Header("FirePoints")]
    [SerializeField] protected List<Transform> firePoints;
    [SerializeField] protected List<Transform> fireDirection;
    [SerializeField] protected Vector3 orientationOffset;
    [Header("Randomize")]
    [SerializeField] protected float randomizeReloadTime;
    [SerializeField] protected float randomizeAngleOfAiming;


    protected bool reloading = true;
    protected float recoilTimeCurrent = 0f;
    [HideInInspector] public int ammoCurrent;
    [HideInInspector] public float reloadTimeCurrent = 0f;
    [HideInInspector] public float loadTimeCurrent = 0f;

    [HideInInspector] public bool finishedFiring = true;
    // Start is called before the first frame update

    public Vector3 GetFirePoint(){
        return firePoints[0].position;
    }

    protected void Awake(){
        loadTimeCurrent = loadTime;
    }

    void FixedUpdate()
    {
        if(recoilTimeCurrent > 0){
            recoilTimeCurrent -= Time.fixedDeltaTime;
        }
        if(reloading){
            reloadTimeCurrent -= Time.fixedDeltaTime;
            if(reloadTimeCurrent <= 0){
                reloading = false;
                ammoCurrent = ammoMax;
            }
        }
    }

    public float GetReloadProportion(){
        return reloadTimeCurrent/reloadTime;
    }

    public void Reload(){
        loadTimeCurrent = loadTime;
        if(!reloading && ammoCurrent < ammoMax){
            reloading = true;
            reloadTimeCurrent = reloadTime + Random.Range(0,randomizeReloadTime);
        }
    }

    public virtual void OnHold(){
        if(ammoCurrent > 0){
            reloadTimeCurrent = 0f;
        }
        Fire();
    }

    public virtual void OnRelease(){
        Fire();
    }

    public void Fire(){//called in FixedUpdate
        if(recoilTimeCurrent <= 0 && ammoCurrent > 0 && loadTimeCurrent <= 0){
            reloading = false;
            recoilTimeCurrent = recoilTime;
            ammoCurrent -= 1;
            for (int i = 0; i < firePoints.Count; i ++){
                Vector3 randomizeValue = new Vector3(Random.Range(0,randomizeAngleOfAiming), Random.Range(0,randomizeAngleOfAiming), 0);
                if(fireDirection != null){
                    if(fireDirection.Count > i){
                        Instantiate(ammo, firePoints[i].position, fireDirection[i].rotation * Quaternion.Euler(orientationOffset) * Quaternion.Euler(randomizeValue));
                    }
                }
                else{
                    Instantiate(ammo, firePoints[i].position, firePoints[i].rotation * Quaternion.Euler(orientationOffset) * Quaternion.Euler(randomizeValue));
                }
            }
        }
        if(ammoCurrent <= 0){
            Reload();
        }
        if(loadTimeCurrent > 0){
            loadTimeCurrent -= Time.fixedDeltaTime;
        }
    } 
}
