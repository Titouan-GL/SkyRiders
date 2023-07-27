using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]private float life = 100f;
    [SerializeField]private GameObject instantiateOnDeath;
    [SerializeField]private bool DisappearOnDeath = true;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(life <= 0){
            Instantiate(instantiateOnDeath, transform.position, transform.rotation);
            if(DisappearOnDeath)Destroy(gameObject);
        }
    }

    public void takeDamage(float damagesDealt){
        life -= damagesDealt;
        animator.CrossFadeInFixedTime("KnockBack", 0.1f);
    }
}
