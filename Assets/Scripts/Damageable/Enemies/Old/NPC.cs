/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]private float life = 100f;
    [SerializeField]public float speed = 18;
    [SerializeField]private GameObject instantiateOnDeath;
    [SerializeField]private bool DisappearOnDeath = true;
    Animator animator;

    [SerializeField]public float loseSightDistance = 500;
    [SerializeField]public float sightDistance = 300;
    [SerializeField]public float fireDistance = 200;
    [SerializeField]public float noStrafeDistance = 30;
    [SerializeField]public float meleeDistance = 10;
    [SerializeField]public float stopForward = 7;
    [SerializeField]public float fleeDistance = 5;
    [SerializeField]public float knockbackTimeMax = 0.4f;
    [SerializeField]public float parriedTime = 0.8f;
    [SerializeField]public float timeBeforeNextStagger = 1f;


    public float knockbackTime = 0f;
    private bool alive = true;
    private float nextStaggerTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsAlive(){
        return alive;
    }

    public void Explodes(){
        Instantiate(instantiateOnDeath, transform.position, transform.rotation);
        if(DisappearOnDeath)Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        knockbackTime -= Time.fixedDeltaTime;
        nextStaggerTime -= Time.fixedDeltaTime;
        if(life <= 0){
            alive = false;
            animator.CrossFadeInFixedTime("Death", 0.1f);
            life = 1;
        }
    }

    public void takeDamage(float damagesDealt, bool knockback = false){
        if(IsAlive()){
            life -= damagesDealt;
            if(knockback){
                Staggered();
            }
        }
    }

    public void Staggered(){
        if(nextStaggerTime <= 0){
            animator.CrossFadeInFixedTime("KnockBack", 0.1f);
            knockbackTime = knockbackTimeMax;
            nextStaggerTime = timeBeforeNextStagger;
        }
    }

    public void Parried(){
        animator.CrossFadeInFixedTime("Parried", 0.1f);
        knockbackTime = parriedTime;
    }
}
*/