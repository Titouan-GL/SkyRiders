using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public List<Transform> targets = new List<Transform>();
    Transform cameraTransform;
    UtilitiesNonStatic UNS;
    [SerializeField] LayerMask layerMaskFire;

    private UIScript uiScript;

    void Awake(){
        base.Awake();
        UNS = UtilitiesStatic.GetUNS();
        cameraTransform = UNS.playerCamera.transform;
        uiScript = UNS.UIObject.GetComponent<UIScript>();
    }

    override public void OnHold(){
        if(ammoCurrent > 0){
            reloadTimeCurrent = 0f;
        }
        RaycastHit hit;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200f, layerMaskFire)){
            GameObject other = hit.collider.gameObject;
            if(other.GetComponentInChildren<Damageable>() && !targets.Contains(other.transform)){
                targets.Add(other.transform);
            }
        }

        //UI
        for(int i = 0; i < Mathf.Min(ammoCurrent, targets.Count); i ++){
            // Convert world position to screen (pixel) position
            if(targets[i] != null){
                Vector3 viewportPosition = UNS.playerCamera.WorldToViewportPoint(targets[i].position);
                UNS.UIObject.GetComponent<UIScript>().UpdateRocketTargetPosition(i, viewportPosition);
            }
        }

        uiScript.RocketAim();
    }

    override public void OnRelease(){
        StartCoroutine(PlayerFire());
    }

    public void Fire(Transform target){
        if(recoilTimeCurrent <= 0 && ammoCurrent > 0 && loadTimeCurrent <= 0){
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
        if(ammoCurrent <= 0){
            Reload();
        }
        if(loadTimeCurrent > 0){
            loadTimeCurrent -= Time.fixedDeltaTime;
        }
    } 

    private IEnumerator PlayerFire(){
        finishedFiring = false;
        while (ammoCurrent > 0){
            if(targets.Count > 0){
                FireOverriden(targets[ammoCurrent%targets.Count]);
            }
            else{
                FireOverriden(null);
            }
            ammoCurrent -= 1;
            yield return new WaitForSeconds(recoilTime);
        }
        targets.Clear();
        Reload();
        finishedFiring = true;
    }

    private void FireOverriden(Transform target){
        reloading = false;
        recoilTimeCurrent = recoilTime;
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
}
