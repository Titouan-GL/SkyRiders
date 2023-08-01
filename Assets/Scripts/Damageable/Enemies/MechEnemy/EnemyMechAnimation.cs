using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyMechAnimation : EnemyAnimation
{
    protected UtilitiesNonStatic UNS;

    [SerializeField]private GameObject instantiateOnDeath;
    [SerializeField]private bool disappearOnDeath;
    [SerializeField] private float defaultLifetimeParticle;
    [SerializeField]private List<ParticleSystem> flamesFront;
    [SerializeField]private List<ParticleSystem> flamesRight;
    [SerializeField]private List<ParticleSystem> flamesLeft;
    [SerializeField]private List<ParticleSystem> flamesBack;
    [SerializeField]private List<ParticleSystem> flamesMecha;

    private Transform aimTarget;
    private MultiAimConstraint aimData;

    private void ModifyParticle(List<ParticleSystem> lps, float value){
        foreach(ParticleSystem ps in lps){
            var main = ps.main;
            main.startLifetime = Mathf.Clamp01(value)*defaultLifetimeParticle;
        }
    }
    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        aimData = GetComponentInChildren<MultiAimConstraint>();
        UNS = UtilitiesStatic.GetUNS();
    }


    public void SetAimTarget(Transform transform){
        aimTarget = transform;
    }

    public void Aiming(bool aiming){
        if(aiming){
            aimData.weight = Mathf.Lerp(aimData.weight, 1f, 0.2f);
            animator.SetLayerWeight(1, aimData.weight);
        }
        else{
            aimData.weight = Mathf.Lerp(aimData.weight, 0f, 0.05f);
            animator.SetLayerWeight(1, aimData.weight);
        }
    }

    public void DetectSliding(bool isJumping){
        animator.SetBool("Slide", false);
        if(isJumping){
            RaycastHit hit;
            animator.SetFloat("slideX", 0);
            Debug.DrawRay(transform.position, transform.right * 2f, Color.white);
            if(Physics.Raycast(transform.position, transform.right, out hit, 2f, UNS.layerMaskLevel)){
                animator.SetFloat("slideX", 1);
                transform.rotation = Quaternion.LookRotation(hit.normal)* Quaternion.Euler(0f, 90f, 0f);
                animator.SetBool("Slide", true);
            }
            Debug.DrawRay(transform.position, transform.right * -2f, Color.white);
            if(Physics.Raycast(transform.position, transform.right*-1, out hit, 2f, UNS.layerMaskLevel)){
                animator.SetFloat("slideX", -1);
                transform.rotation = Quaternion.LookRotation(hit.normal)* Quaternion.Euler(0f, -90f, 0f);
                animator.SetBool("Slide", true);
            }
        }
    }

    
    public void UpdateParticles(){
        ModifyParticle(flamesMecha, 1);
        ModifyParticle(flamesFront, Mathf.Clamp01(animationAxis.y));
        ModifyParticle(flamesRight, Mathf.Clamp01(animationAxis.x));
        ModifyParticle(flamesLeft, Mathf.Clamp01(-animationAxis.x));
        ModifyParticle(flamesBack, Mathf.Clamp01(-animationAxis.y));
    }

    public void SetComboInteger(int combo){
        animator.SetInteger("Combo", combo); 
    }

    public void Dies(){
        animator.CrossFadeInFixedTime("Death", 0.1f);
    }
    
    public void Explodes(){
        Instantiate(instantiateOnDeath, transform.position, transform.rotation);
        if(disappearOnDeath)Destroy(gameObject);
    }

    public void AnimationParried(){
        animator.CrossFadeInFixedTime("Parried", 0.1f);
    }
    public void AnimationStaggered(){
        animator.CrossFadeInFixedTime("KnockBack", 0.1f);
    }

}
