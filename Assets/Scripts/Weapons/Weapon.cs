using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject ammo;
    [SerializeField] protected string weaponName;
    [SerializeField] public float recoilTime = 0.1f;
    [SerializeField] public float reloadTime = 2f;
    [SerializeField] public int ammoMax = 10;
    [SerializeField] protected List<Transform> firePoints;
    [SerializeField] protected List<Transform> fireDirection;
    [SerializeField] protected Vector3 orientationOffset;
    protected float recoilTimeCurrent = 0f;
    [HideInInspector]public float reloadTimeCurrent = 0f;
    protected bool reloading = true;
    [HideInInspector] public int ammoCurrent;

    [SerializeField] protected float randomizeReloadTime;
    [SerializeField] protected float randomizeAngleOfAiming;
    // Start is called before the first frame update

    public Vector3 GetFirePoint(){
        return firePoints[0].position;
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

    public void Reload(){
        if(!reloading){
            reloading = true;
            reloadTimeCurrent = reloadTime + Random.Range(0,randomizeReloadTime);
        }
    }

    public virtual void OnHold(){
        Fire();
    }

    public virtual void OnRelease(){
        Fire();
    }

    public void Fire(){
        if(recoilTimeCurrent <= 0 && ammoCurrent > 0){
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
        else if(ammoCurrent <= 0){
            Reload();
        }
    } 
}
