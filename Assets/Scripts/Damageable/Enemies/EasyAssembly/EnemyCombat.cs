using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : Damageable
{
    
    [SerializeField]public float knockbackTimeMax = 0.4f;
    [SerializeField]public float timeBeforeNextStagger = 1f;


    [HideInInspector]public float knockbackTime = 0f;
    [HideInInspector]public float nextStaggerTime = 0f;

    override public void Awake(){
        base.Awake();
    }


    public virtual void Staggered(){
        if(nextStaggerTime <= 0){
            knockbackTime = knockbackTimeMax;
            nextStaggerTime = timeBeforeNextStagger;
        }
    }

}
