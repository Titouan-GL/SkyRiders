using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsManager : MonoBehaviour
{
    [HideInInspector] public PlayerAnimation animationScript;
    [HideInInspector] public PlayerMovement movementScript;
    [HideInInspector] public PlayerCombat combatScript;
    [HideInInspector] public CameraScript cameraScript;
    [HideInInspector] public UIScript UIScript;
    [HideInInspector] public Damageable damageable;

    [HideInInspector] public bool guard = false;
    [HideInInspector] public bool aiming = false;
    [HideInInspector] public bool flying = false;
    [HideInInspector] public bool sliding = false;
    [HideInInspector] public bool firing = false;
    [HideInInspector] public int combo = 0;
    
    [HideInInspector] public Inputs inputBuffered;
    [HideInInspector] public Inputs inputHeld;
    [HideInInspector] public Inputs attackHeld;
    [HideInInspector] public Inputs attackReleased;
    private float bufferTimeMax = 0.2f;
    private float bufferTimeCurrent = 0.2f;

    private float horizontalInput;
    private float verticalInput;
    private Vector2 animationInput;

    PlayerBaseState currentState;
    [HideInInspector]public PlayerStateMoving movingState = new PlayerStateMoving();
    [HideInInspector]public PlayerStateInAir inAirState = new PlayerStateInAir();
    [HideInInspector]public PlayerStateSliding slidingState = new PlayerStateSliding();
    [HideInInspector]public PlayerStateFlying flyingState = new PlayerStateFlying();
    [HideInInspector]public PlayerStateGuard guardState = new PlayerStateGuard();
    [HideInInspector]public PlayerStateMelee meleeState = new PlayerStateMelee();
    [HideInInspector]public PlayerStateAiming aimState = new PlayerStateAiming();

    UtilitiesNonStatic UNS;

    public enum Inputs{
        None,
        Attack,
        Fly,
        Guard,
        Aim,
        Jump
    }

    void Awake(){
        animationScript = GetComponent<PlayerAnimation>();
        movementScript = GetComponent<PlayerMovement>();
        combatScript = GetComponent<PlayerCombat>();
        damageable = GetComponent<Damageable>();
        currentState = movingState;
        UNS = UtilitiesStatic.GetUNS();
        cameraScript = UNS.playerCamera.gameObject.GetComponentInChildren<CameraScript>();
        UIScript = UNS.UIObject.GetComponent<UIScript>();

        UIScript.InitializeAmmo(combatScript.weaponMech1.ammoMax, 0, combatScript.weaponMech1.ammoUI);
        UIScript.InitializeAmmo(combatScript.weaponMech2.ammoMax, 1, combatScript.weaponMech2.ammoUI);
    }

    void Start(){
        currentState.EnterState(this);
    }

    public void UpdateUI(){
        UIScript.UpdateRecharges(combatScript.GetReloadWeapon1(), combatScript.GetReloadWeapon2());
        UIScript.UpdateFuel(movementScript.fuelRatio());
        UIScript.UpdateLifeBar(damageable.GetLifeRatio());
        UIScript.UpdateAmmo(combatScript.weaponMech1.ammoCurrent, combatScript.weaponMech2.ammoCurrent);
    }
    
    public void SwitchState(PlayerBaseState state){
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public Vector2 UpdateAnimationAxis(){
        //should only be called once per frames of course;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        animationInput = Vector2.Lerp(animationInput, new Vector2(horizontalInput, verticalInput), 0.1f);
        return animationInput;
    }

    public Vector2 GetAnimationAxis(){
        return animationInput;
    }

    void Update()
    {
        //InputBuffer
        if(bufferTimeCurrent <= 0){
            inputBuffered = Inputs.None;
        }
        else if (inputBuffered != Inputs.None){
            bufferTimeCurrent -= Time.deltaTime;
        }


        //input buffer
        //input entered once
        if(Input.GetButtonDown("Transform")){
            inputBuffered = Inputs.Fly;
            bufferTimeCurrent = bufferTimeMax;
        }
        /*else if(Input.GetButtonDown("Jump")){
            inputBuffered = Inputs.Jump;
            bufferTimeCurrent = bufferTimeMax;
        }*/
        else if(Input.GetButtonDown("Fire1")){
            inputBuffered = Inputs.Attack;
            bufferTimeCurrent = bufferTimeMax;
        }
        //input held
        if(Input.GetButton("Aim")){
            inputHeld = Inputs.Aim;
        }
        else{
            inputHeld = Inputs.None;
        }
        //attack held
        if (Input.GetButton("Fire2")){
            attackHeld = Inputs.Guard;
        }
        else if(Input.GetButton("Fire1")){
            attackHeld = Inputs.Attack;
        }
        else{
            attackHeld = Inputs.None;
        }
        //attack released
        if (Input.GetButtonUp("Fire2")){
            attackReleased = Inputs.Guard;
        }
        else if(Input.GetButtonUp("Fire1")){
            attackReleased = Inputs.Attack;
        }
        else{
            attackReleased = Inputs.None;
        }

        currentState.UpdateState(this);

    }

    void FixedUpdate()
    {
        currentState.UpdatePhysics(this);
    }

    /*private void UnusedUpdate(){

        //guard
        guard = Input.GetButton("Fire2") && !aiming;
        if(guard){
            animationScript.Guard();
            flying = false; 
            sliding = false;
            combo = 0;
        }

        //aim
        animationScript.Aiming();
        if(Input.GetButton("Aim") && !flying && !guard && !sliding){
            inputBuffered = Inputs.None;
            if(Input.GetButton("Fire2")){
                combatScript.Fire2Hold();
                firing = true;
            }
            else if(Input.GetButton("Fire1")){
                combatScript.Fire1Hold();
                firing = true;
            }
            else if(Input.GetButtonUp("Fire2")){
                combatScript.Fire2Release();
            }
            else if(Input.GetButtonUp("Fire1")){
                combatScript.Fire1Release();
            }
            else{
                firing = false;
            }
        }
        else if(Input.GetButton("Fire1") && flying){//aim in plane
            combatScript.PlaneFire1Hold();
            firing = true;
        }
        
        //attack
        //combatScript.UpdateCombo(inputBuffered == Inputs.Attack);

        if(inputBuffered == Inputs.Fly &&  !guard){
            flying = !flying;
            sliding = false;
            combo = 0;
            inputBuffered = Inputs.None;
        }

        animationScript.SetFlying(flying);
        animationScript.SetSliding(sliding);
        animationScript.SetGuard(guard);
        animationScript.SetCombo(combo);
        
        if(flying == false){
            
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            animationInput = Vector2.Lerp(animationInput, new Vector2(horizontalInput, verticalInput), 0.1f);

            animationScript.Sliding();
            animationScript.SetMovementAxis(animationInput);
            animationScript.ModifyAllParticles(animationInput);

            if(inputBuffered == Inputs.Jump){
                movementScript.Jump();
                inputBuffered = Inputs.None;
            }
        }
        else{
            animationScript.ModifyAllParticles();
        }
    }*/

    private void OnApplicationFocus(bool focus){
        if(focus){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
