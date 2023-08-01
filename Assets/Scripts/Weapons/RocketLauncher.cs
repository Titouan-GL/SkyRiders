using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public List<Transform> targets = new List<Transform>();
    Transform cameraTransform;
    UtilitiesNonStatic UNS;
    [SerializeField] LayerMask layerMaskFire;

    void Awake(){
        UNS = UtilitiesStatic.GetUNS();
        cameraTransform = UNS.playerCamera.transform;
    }

    override public void OnHold(){
        RaycastHit hit;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200f, layerMaskFire)){
            GameObject other = hit.collider.gameObject;
            if(other.GetComponentInChildren<Damageable>() && !targets.Contains(other.transform)){
                targets.Add(other.transform);
            }
        }
    }

    override public void OnRelease(){
        foreach(Transform target in targets){
            Fire(target);
        }
    }

    public void Fire(Transform target){
        if(recoilTimeCurrent <= 0 && ammoCurrent > 0){
            reloading = false;
            recoilTimeCurrent = recoilTime;
            ammoCurrent -= 1;
            for (int i = 0; i < firePoints.Count; i ++){
                Vector3 randomizeValue = new Vector3(Random.Range(0,randomizeAngleOfAiming), Random.Range(0,randomizeAngleOfAiming), 0);
                if(fireDirection != null){
                    if(fireDirection.Count > i){
                        GameObject go = Instantiate(ammo, firePoints[i].position, fireDirection[i].rotation * Quaternion.Euler(orientationOffset) * Quaternion.Euler(randomizeValue));
                        go.GetComponentInChildren<Rocket>().target = target;      
                    }
                }
                else{
                    GameObject go = Instantiate(ammo, firePoints[i].position, firePoints[i].rotation * Quaternion.Euler(orientationOffset) * Quaternion.Euler(randomizeValue));
                    go.GetComponentInChildren<Rocket>().target = target;
                }
            }
        }
        else if(ammoCurrent <= 0){
            Reload();
        }
    } 
}
