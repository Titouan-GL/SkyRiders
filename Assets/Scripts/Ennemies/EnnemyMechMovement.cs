using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMechMovement : MonoBehaviour
{
    Vector3 velocity = new Vector3();
    [SerializeField] private float gravityScale = 3f;
    private float ySpeed;
    private CharacterController characterController;
    private float defaultLifetimeParticle = 0.5f;
    [SerializeField] private LayerMask layerMaskLevel;

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
        characterController = GetComponent<CharacterController>();
    }

    void ModifyParticle(List<ParticleSystem> lps, float value){
        foreach(ParticleSystem ps in lps){
            var main = ps.main;
            main.startLifetime = Mathf.Clamp01(value)*defaultLifetimeParticle;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //velocity = Vector3.Lerp(velocity, movementDirection * speed, 0.9f);
        
        ySpeed += Physics.gravity.y * Time.deltaTime * gravityScale;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(-1*Vector3.up), out hit, 3.1f, layerMaskLevel))
        {
            if(ySpeed < 0){
                ySpeed = 0.0f;
            }
        }


        velocity.y = ySpeed;
        transform.position= transform.position + ( velocity * Time.fixedDeltaTime); 

        ModifyParticle(flamesPlane, 0);
        ModifyParticle(flamesMecha, 1);
        ModifyParticle(flamesFront, Mathf.Clamp01(velocity.z));
        ModifyParticle(flamesRight, Mathf.Clamp01(velocity.x));
        ModifyParticle(flamesLeft, Mathf.Clamp01(-velocity.x));
        ModifyParticle(flamesBack, Mathf.Clamp01(-velocity.z));
    }
}
