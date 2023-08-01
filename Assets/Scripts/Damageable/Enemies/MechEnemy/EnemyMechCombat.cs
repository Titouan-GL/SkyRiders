using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechCombat : EnemyCombat, MeleeCombat, RangedCombat
{

    [SerializeField] protected GameObject slashCollider;
    [SerializeField]public float parriedTime = 0.8f;
    [SerializeField] protected Weapon distanceWeapon;

    [HideInInspector]public bool aiming;
    [HideInInspector]public bool updateCombo;
    [HideInInspector]public int meleeCombo;
    
    private MechStateManager MSM;

    override public void Awake(){
        base.Awake();
        MSM = GetComponent<MechStateManager>();
    }

    void FixedUpdate()
    {
        knockbackTime -= Time.fixedDeltaTime;
        nextStaggerTime -= Time.fixedDeltaTime;
        if(life <= 0 && alive == true){
            MSM.animationScript.Dies();
            alive = false;
        }
    }

    public void ceaseMelee(){
        meleeCombo = 0;
    }
    

    override public void takeDamage(float damagesDealt, bool knockback = false){
        if(IsAlive()){
            life -= damagesDealt;
            if(knockback){
                Staggered();
            }
        }
    }

    public void UpdateCombo(){
        updateCombo = true;
    }

    public void ChangeAttack(){
        if(meleeCombo < 2){
            meleeCombo += 1;
        }
        else{
            meleeCombo = 1;
        }
    }

    override public void Staggered(){
        if(nextStaggerTime <= 0){
            knockbackTime = knockbackTimeMax;
            nextStaggerTime = timeBeforeNextStagger;
            UpdateCombo();
            ceaseMelee();
            MSM.animationScript.AnimationStaggered();
        }
    }

    public void Parried(){
        knockbackTime = parriedTime;
        MSM.animationScript.AnimationParried();
        UpdateCombo();
        ceaseMelee();
    }

    public void InstantiateSlash(){
        slashCollider.SetActive(false);
        slashCollider.SetActive(true);

    }

    public void TryFire(){
        RaycastHit hit;
        Vector3 raycastDirection = distanceWeapon.GetFirePoint() - transform.position;
        Debug.DrawRay(transform.position, raycastDirection, Color.green);
        if(distanceWeapon && !Physics.Raycast(transform.position, raycastDirection.normalized, out hit, raycastDirection.magnitude, UNS.layerMaskLevel)){
            distanceWeapon.Fire();
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
