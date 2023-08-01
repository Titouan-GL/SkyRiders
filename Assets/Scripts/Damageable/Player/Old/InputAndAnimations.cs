/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAndAnimations //: MonoBehaviour
{
    private float defaultLifetimeParticle = 0.5f;
    public Animator animator;
    public float horizontalInput;
    public float verticalInput;

    public Vector2 animationInput;
    public bool jump = false;
    public bool guard = false;
    public int combo = 0;
    public bool flying = false;
    public bool sliding = false;
    public bool aiming = false;
    public bool firing = false;

    private Inputs inputBuffered;
    private float bufferTimeMax = 0.2f;
    private float bufferTimeCurrent = 0.2f;

    public enum Inputs{
        None,
        Attack,
        Fly,
        Jump
    }



    [SerializeField]
    private List<ParticleSystem> flamesFront;

    [SerializeField]
    private List<ParticleSystem> flamesRight;

    [SerializeField]
    private List<ParticleSystem> flamesLeft;

    [SerializeField]
    private List<ParticleSystem> flamesBack;

    [SerializeField]
    private List<ParticleSystem> flamesMecha;

    [SerializeField]
    private List<ParticleSystem> flamesPlane;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void ModifyParticle(List<ParticleSystem> lps, float value){
        foreach(ParticleSystem ps in lps){
            var main = ps.main;
            main.startLifetime = Mathf.Clamp01(value)*defaultLifetimeParticle;
        }
    }

    public void TakeDamage(){
        animator.CrossFadeInFixedTime("KnockBack", 0.1f);
        combo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //InputBuffer
        if(bufferTimeCurrent <= 0){
            inputBuffered = Inputs.None;
        }
        else if (inputBuffered != Inputs.None){
            bufferTimeCurrent -= Time.deltaTime;
        }

        //guard
        guard = Input.GetButton("Fire2") && !aiming;
        if(guard){
            animator.CrossFadeInFixedTime("Mech_Guard", 0.1f);
            flying = false; 
            sliding = false;
            combo = 0;
        }

        //aim
        if(Input.GetButton("Aim") && !flying && !guard && !sliding){
            aiming = true;
            if(Input.GetButton("Fire1")){
                firing = true;
                inputBuffered = Inputs.None;
            }
            else{
                firing = false;
            }
        }
        else if(Input.GetButton("Fire1") && flying){//aim in plane
            firing = true;
        }
        else{
            aiming = false;
            firing = false;
        }

        //input buffer
        if(Input.GetButtonDown("Jump")){
            inputBuffered = Inputs.Jump;
            bufferTimeCurrent = bufferTimeMax;
        }
        else if(Input.GetButtonDown("Fire1")){
            inputBuffered = Inputs.Attack;
            bufferTimeCurrent = bufferTimeMax;
        }
        else if(Input.GetButtonDown("Transform")){
            inputBuffered = Inputs.Fly;
            bufferTimeCurrent = bufferTimeMax;
        }

        //attack
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        if(asi.IsName("Attack_Combo_1") && combo == 1 && !aiming){
            if(asi.normalizedTime >= 1f) {
                combo = 0;
            }
            else if (inputBuffered == Inputs.Attack && guard == false && asi.normalizedTime >= 0.5f){
                combo = 2;
                sliding = false;
            }
        }
        else if(asi.IsName("Attack_Combo_2") && combo == 2 && !aiming){
            if(asi.normalizedTime >= 1f) {
                combo = 0;
            }
            else if (inputBuffered == Inputs.Attack && guard == false && asi.normalizedTime >= 0.7f){
                combo = 1;
                sliding = false;
                inputBuffered = Inputs.None;
            }
        }
        else if(combo == 0 && !flying && !aiming){
            if (inputBuffered == Inputs.Attack && guard == false){
                combo = 1;
                sliding = false;
                inputBuffered = Inputs.None;
            }
        }
        else if(aiming && asi.normalizedTime >= 1f){
            combo = 0;
        }

        if(inputBuffered == Inputs.Fly &&  !guard){
            flying = !flying;
            sliding = false;
            combo = 0;
            inputBuffered = Inputs.None;
        }


        animator.SetBool("Plane", flying); 
        animator.SetBool("Slide", sliding); 
        animator.SetBool("Guard", guard); 
        animator.SetInteger("Combo", combo); 
        
        if(flying == false){
            
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            animationInput = Vector2.Lerp(animationInput, new Vector2(horizontalInput, verticalInput), 0.1f);
            animator.SetFloat("x", animationInput.x);
            animator.SetFloat("y", animationInput.y);

            ModifyParticle(flamesPlane, 0);
            ModifyParticle(flamesMecha, 1);
            ModifyParticle(flamesFront, animationInput.y);
            ModifyParticle(flamesRight, animationInput.x);
            ModifyParticle(flamesLeft, animationInput.x * -1);
            ModifyParticle(flamesBack, animationInput.y * -1);
            if(inputBuffered == Inputs.Jump){
                jump = true;
                inputBuffered = Inputs.None;
            }
        }
        else{
            ModifyParticle(flamesPlane, 1);
            ModifyParticle(flamesMecha, 0);
            ModifyParticle(flamesFront, 0);
            ModifyParticle(flamesRight, 0);
            ModifyParticle(flamesLeft, 0);
            ModifyParticle(flamesBack, 0);
        }


    }
}*/
