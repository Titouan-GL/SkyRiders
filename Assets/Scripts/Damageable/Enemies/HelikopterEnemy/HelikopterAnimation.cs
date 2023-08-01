using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HelikopterAnimation : EnemyAnimation
{
    protected UtilitiesNonStatic UNS;

    [SerializeField]private GameObject instantiateOnDeath;
    [SerializeField]private bool disappearOnDeath;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        UNS = UtilitiesStatic.GetUNS();
    }


    public void Dies(){
        //animator.CrossFadeInFixedTime("Death", 0.1f);
        Explodes();
    }
    
    public void Explodes(){
        Instantiate(instantiateOnDeath, transform.position, transform.rotation);
        if(disappearOnDeath)Destroy(gameObject);
    }

    public void AnimationStaggered(){
        //animator.CrossFadeInFixedTime("KnockBack", 0.1f);
    }

}
