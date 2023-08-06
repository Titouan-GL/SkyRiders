using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float life;
    private float lifeStart;
    protected UtilitiesNonStatic UNS;

    [HideInInspector]public bool alive = true;

    public virtual void Awake(){
        UNS = UtilitiesStatic.GetUNS();
        lifeStart = life;
    }

    public float GetLifeRatio(){
        return life/lifeStart;
    }

    public bool IsAlive(){
        return alive;
    }

    public virtual void takeDamage(float damagesDealt, bool knockback = false){
        if(IsAlive()){
            life -= damagesDealt;
        }
    }
}
