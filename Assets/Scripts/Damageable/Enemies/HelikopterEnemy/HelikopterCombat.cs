using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelikopterCombat : EnemyCombat, RangedCombat
{
    [SerializeField] protected Weapon distanceWeapon;
    [SerializeField] protected RocketLauncher rocketLauncher;

    [HideInInspector]public bool aiming;
    
    private HelikopterStateManager HSM;

    override public void Awake(){
        base.Awake();
        HSM = GetComponent<HelikopterStateManager>();
    }

    void FixedUpdate()
    {
        knockbackTime -= Time.fixedDeltaTime;
        nextStaggerTime -= Time.fixedDeltaTime;
        if(life <= 0 && alive == true){
            HSM.animationScript.Dies();
            alive = false;
        }
    }

    public void TryFire(){
        RaycastHit hit;
        Vector3 raycastDirection = distanceWeapon.GetFirePoint() - transform.position;
        Debug.DrawRay(transform.position, raycastDirection, Color.green);
        if(distanceWeapon && !Physics.Raycast(transform.position, raycastDirection.normalized, out hit, raycastDirection.magnitude, UNS.layerMaskLevel)){
            distanceWeapon.Fire();
            rocketLauncher.Fire(UNS.player.transform);
        }
    }

    public void TestAiming(){
        float aimingTime = 0.5f;
        aiming = (distanceWeapon.reloadTimeCurrent < aimingTime || distanceWeapon.reloadTimeCurrent > distanceWeapon.reloadTime - aimingTime);
    }

    public void SetAiming(bool boolean){
        aiming = boolean;
    }
}
