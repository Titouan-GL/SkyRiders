using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] private InputsManager inputs;
    [HideInInspector] private MultiAimConstraint aimData;


    private float defaultLifetimeParticle = 0.5f;
    [SerializeField] private List<ParticleSystem> flamesFront;
    [SerializeField] private List<ParticleSystem> flamesRight;
    [SerializeField] private List<ParticleSystem> flamesLeft;
    [SerializeField] private List<ParticleSystem> flamesBack;
    [SerializeField] private List<ParticleSystem> flamesMecha;
    [SerializeField] private List<ParticleSystem> flamesPlane;

    UtilitiesNonStatic UNS; 
    
    void Start()
    {
        animator = GetComponent<Animator>();
        inputs = GetComponent<InputsManager>();
        aimData = GetComponentInChildren<MultiAimConstraint>();
        UNS = UtilitiesStatic.GetUNS();
    }

    void ModifyParticle(List<ParticleSystem> lps, float value){
        foreach(ParticleSystem ps in lps){
            var main = ps.main;
            main.startLifetime = Mathf.Clamp01(value)*defaultLifetimeParticle;
        }
    }

    public void Guard(){
        animator.CrossFadeInFixedTime("Mech_Guard", 0.1f);
    }

    public void Aiming(){
        aimData.weight = Mathf.Lerp(aimData.weight, 1f, 0.3f);
        animator.SetLayerWeight(1, aimData.weight);
    }

    public void NotAiming(){
        aimData.weight = Mathf.Lerp(aimData.weight, 0f, 0.3f);
        animator.SetLayerWeight(1, aimData.weight);
    }

    public void SetMovementAxis(Vector2 animationInput){
        animator.SetFloat("x", animationInput.x);
        animator.SetFloat("y", animationInput.y);
    }


    public void SetFlying(bool flying){
        animator.SetBool("Plane", flying); 
    }
    public void SetSliding(bool sliding){
        animator.SetBool("Slide", sliding); 
    }
    public void SetGuard(bool guard){
        animator.SetBool("Guard", guard); 
    }
    public void SetCombo(int combo){
        animator.SetInteger("Combo", combo); 
    }


    public bool Sliding(){
        bool isSliding = false;
        //slide detection
        RaycastHit hit2;
        inputs.sliding = false;
        animator.SetFloat("slideX", 0);
        Debug.DrawRay(transform.position, transform.right * 2f, Color.white);
        if(Physics.Raycast(transform.position, transform.right, out hit2, 2f, UNS.layerMaskLevel)){
            isSliding = true;
            animator.SetFloat("slideX", 1);
            transform.rotation = Quaternion.LookRotation(hit2.normal)* Quaternion.Euler(0f, 90f, 0f);
        }
        Debug.DrawRay(transform.position, transform.right * -2f, Color.white);
        if(Physics.Raycast(transform.position, transform.right*-1, out hit2, 2f, UNS.layerMaskLevel)){
            isSliding = true;
            animator.SetFloat("slideX", -1);
            transform.rotation = Quaternion.LookRotation(hit2.normal)* Quaternion.Euler(0f, -90f, 0f);
        }  
        animator.SetBool("Slide", isSliding);
        return isSliding;
    }

    public void StopSliding(){
        animator.SetBool("Slide", false);
    }
    
    public void ModifyAllParticles(Vector2 animationInput){
            ModifyParticle(flamesPlane, 0);
            ModifyParticle(flamesMecha, 1);
            ModifyParticle(flamesFront, animationInput.y);
            ModifyParticle(flamesRight, animationInput.x);
            ModifyParticle(flamesLeft, animationInput.x * -1);
            ModifyParticle(flamesBack, animationInput.y * -1);
    }

    public void ModifyAllParticles(){
            ModifyParticle(flamesPlane, 1);
            ModifyParticle(flamesMecha, 0);
            ModifyParticle(flamesFront, 0);
            ModifyParticle(flamesRight, 0);
            ModifyParticle(flamesLeft, 0);
            ModifyParticle(flamesBack, 0);
    }

}
