/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Animations.Rigging;
//using UnityEngine.AI;

public class EnnemyMechMovement //: MonoBehaviour
{
    [SerializeField] private float gravityScale = 9f;
    [SerializeField] private float jumpSpeed = 40f;
    private float ySpeed;
    private float defaultLifetimeParticle = 0.5f;
    [SerializeField] private LayerMask layerMaskLevel;
    private Animator animator;
    [SerializeField] private MultiAimConstraint aimData;
    [SerializeField] private Weapon distanceWeapon;
    public Transform playerTransform;
    private Transform target;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private NPC npcInfo;
    public bool sighted = false;
    private bool aiming = true;
    private Vector2 animationAxis;
    public int combo = 0;
    private Vector3 previousPosition;
    private bool navMeshControl;
    public float playerDistance;

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

    [SerializeField] private List<GameObject> slashes;
    [SerializeField] private GameObject slashCollider;

    Vector2 strafe;
    Vector2 nextStrafe;
    float strafeTime = 0f;
    [SerializeField] float strafeTimeMin = 1f;
    [SerializeField] float strafeTimeMax = 4f;

    [SerializeField] float jumpIntervalMin = 5f;
    [SerializeField] float jumpIntervalMax = 10f;
    private float jumpIntervalCurrent = 10f;

    public float baseNavMeshOffset;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        aimData = GetComponentInChildren<MultiAimConstraint>();
        target = aimData.transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        npcInfo = GetComponent<NPC>();
        npcInfo.stopForward = navMeshAgent.stoppingDistance;
        navMeshAgent.speed = npcInfo.speed - Random.Range(0, npcInfo.speed/10);
        jumpIntervalCurrent = Random.Range(jumpIntervalMin, jumpIntervalMax);
        baseNavMeshOffset = navMeshAgent.baseOffset;
    }

    void ModifyParticle(List<ParticleSystem> lps, float value){
        foreach(ParticleSystem ps in lps){
            var main = ps.main;
            main.startLifetime = Mathf.Clamp01(value)*defaultLifetimeParticle;
        }
    }

    void CeaseMelee(AnimatorStateInfo asi){
        if(asi.IsName("Attack_Combo_1")){
            if(asi.normalizedTime >= 1f) {
                combo = 0;
            }
        }
        else if(asi.IsName("Attack_Combo_2")){
            if(asi.normalizedTime >= 1f) {
                combo = 0;
            }
        }
        else{
            combo = 0;
        }
    }

    private Vector2 GetAnimationAxis(){
        Vector3 positionMoved = previousPosition - transform.position;
        
        Vector3 localDirection = transform.InverseTransformDirection(positionMoved).normalized;

        previousPosition = transform.position;
        return new Vector2(localDirection.x*-1, localDirection.z*-1);

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(npcInfo.knockbackTime > 0 || !npcInfo.IsAlive()){
            distanceWeapon.ammoCurrent = 0;
            combo = 0;
            navMeshAgent.destination = transform.position;
        }
        else{
            //animInfo
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);


            //behavior depending on distance
            playerDistance = Vector3.Distance(transform.position, playerTransform.position);
            navMeshControl = sighted;

            if(playerDistance < npcInfo.fleeDistance && combo == 0){//taking distance because too close
                /*transform.position = Vector3.Lerp(transform.position, transform.position+transform.forward *-5, 0.03f);
                navMeshControl = false;
            }
            else if(playerDistance < npcInfo.meleeDistance){//melee
                aiming = false;
                if(combo == 1 ){
                    if (asi.IsName("Attack_Combo_1") &&  asi.normalizedTime >= 1f){
                        combo = 2;
                        //sliding = false;
                    }
                }
                else if(combo == 2){
                    if (asi.IsName("Attack_Combo_2") && asi.normalizedTime >= 1f){
                        combo = 0;
                    }
                }
                else{
                    combo = 1;
                    //sliding = false;
                }
            }
            else if(playerDistance < npcInfo.fireDistance){//firing
                //aiming = (distanceWeapon.reloadTimeCurrent < 0.5f || distanceWeapon.reloadTimeCurrent > distanceWeapon.reloadTime - 0.5f) && combo == 0;
                CeaseMelee(asi);
                if(playerDistance > npcInfo.noStrafeDistance){
                    if(jumpIntervalCurrent <= 0){
                        ySpeed = jumpSpeed;
                        jumpIntervalCurrent = Random.Range(jumpIntervalMin, jumpIntervalMax);
                    }
                    else{
                        jumpIntervalCurrent -= Time.fixedDeltaTime;
                    }
                }
            }
            else{//to far to aim
                aiming = false;
                CeaseMelee(asi);
            }

            if(combo > 0 ){
                navMeshControl = false;
            }

            if(playerDistance > npcInfo.loseSightDistance){//not moving forward(sight lost, too close, attacking)
                sighted = false;
                navMeshControl = false;
            }
            else if(playerDistance < npcInfo.sightDistance){//sighted, getting closer
                sighted = true;
            }

            //strafing
            if(strafeTime > 0){
                strafeTime -= Time.fixedDeltaTime;
                strafe = Vector2.Lerp(strafe, nextStrafe, 0.1f);
            }
            else{
                strafeTime = Random.Range(strafeTimeMin, strafeTimeMax);
                nextStrafe = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized * 10;
            }
            

            //test if strafing can occur
            RaycastHit hit2;
            if(navMeshAgent.baseOffset <= baseNavMeshOffset){
                Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + transform.forward) * 10f, Color.red);
                if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) + transform.forward, out hit2, 10f, layerMaskLevel)){
                    if(strafe.y > 0){
                        strafe.y = 0;
                        nextStrafe.y = 0;
                    }
                }
                Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + transform.right) * 10f, Color.red);
                if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) + transform.right, out hit2, 10f, layerMaskLevel)){
                    if(strafe.x > 0){
                        strafe.x = 0;
                        nextStrafe.x = 0;
                    }
                }
                Debug.DrawRay(transform.position, (new Vector3(0f, -1f, 0f) + -transform.right) * 10f, Color.red);
                if(!Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f) - transform.right, out hit2, 10f, layerMaskLevel)){
                    if(strafe.x < 0){
                        strafe.x = 0;
                        nextStrafe.x = 0;
                    }
                }
            }
            

            //rotate towards player
            if(sighted){
                Vector3 direction = target.position - transform.position;
                direction.y = 0f;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.05f);
                target.position = Vector3.Lerp(target.position, playerTransform.position, 0.05f);
            }


            //aim
            if(aiming){
                aimData.weight = Mathf.Lerp(aimData.weight, 1f, 0.2f);
                animator.SetLayerWeight(1, aimData.weight);
                
                RaycastHit hit;
                Vector3 raycastDirection = aimData.transform.position - transform.position;
                Debug.DrawRay(transform.position, raycastDirection, Color.green);
                Debug.Log(navMeshAgent.transform.position + " " + transform.position);
                if(distanceWeapon && !Physics.Raycast(transform.position, raycastDirection.normalized, out hit, raycastDirection.magnitude, layerMaskLevel)){
                    distanceWeapon.Fire();
                }
            }
            else{
                aimData.weight = Mathf.Lerp(aimData.weight, 0f, 0.05f);
                animator.SetLayerWeight(1, aimData.weight);
            }

            //move

            if(navMeshControl){
                navMeshAgent.destination = playerTransform.position;
                transform.position += transform.right * strafe.x * Time.fixedDeltaTime;
                transform.position += transform.forward * strafe.y * Time.fixedDeltaTime;
            }
            else{
                navMeshAgent.destination = transform.position;
            }
            
            //slide detection
            animator.SetBool("Slide", false);
            if(navMeshAgent.baseOffset > baseNavMeshOffset){
                RaycastHit hit;
                animator.SetFloat("slideX", 0);
                Debug.DrawRay(transform.position, transform.right * 2f, Color.white);
                if(Physics.Raycast(transform.position, transform.right, out hit, 2f, layerMaskLevel)){
                    animator.SetFloat("slideX", 1);
                    transform.rotation = Quaternion.LookRotation(hit.normal)* Quaternion.Euler(0f, 90f, 0f);
                    animator.SetBool("Slide", true);
                }
                Debug.DrawRay(transform.position, transform.right * -2f, Color.white);
                if(Physics.Raycast(transform.position, transform.right*-1, out hit2, 2f, layerMaskLevel)){
                    animator.SetFloat("slideX", -1);
                    transform.rotation = Quaternion.LookRotation(hit.normal)* Quaternion.Euler(0f, -90f, 0f);
                    animator.SetBool("Slide", true);
                }
            }

            //animate
            animationAxis = Vector2.Lerp(animationAxis, GetAnimationAxis(), 0.1f);

            ModifyParticle(flamesMecha, 1);
            ModifyParticle(flamesFront, Mathf.Clamp01(animationAxis.y));
            ModifyParticle(flamesRight, Mathf.Clamp01(animationAxis.x));
            ModifyParticle(flamesLeft, Mathf.Clamp01(-animationAxis.x));
            ModifyParticle(flamesBack, Mathf.Clamp01(-animationAxis.y));

            animator.SetFloat("y", animationAxis.y);
            animator.SetFloat("x", animationAxis.x);
        }
        
        animator.SetInteger("Combo", combo); 
        ySpeed += Physics.gravity.y * Time.deltaTime * gravityScale;
        navMeshAgent.baseOffset += ySpeed * Time.fixedDeltaTime;

        if(navMeshAgent.baseOffset < baseNavMeshOffset)
        {
            navMeshAgent.baseOffset = baseNavMeshOffset;
            ySpeed = 0;
        }
    }

    public void InstantiateSlash(int index){
        //slashes[index].SetActive(false);
        //slashes[index].SetActive(true);
        slashCollider.SetActive(false);
        slashCollider.SetActive(true);

    }
}
*/