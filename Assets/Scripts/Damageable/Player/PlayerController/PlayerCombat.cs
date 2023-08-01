using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [HideInInspector] private InputsManager inputs;

    [SerializeField] private Weapon weaponMech1;
    [SerializeField] private Weapon weaponMech2;
    [SerializeField] private Weapon weaponPlane1;

    [SerializeField] private GameObject guardCollider;
    [SerializeField] private List<GameObject> slashes;
    [SerializeField] private GameObject slashCollider;
    
    public int meleeCombo;
    [HideInInspector]public bool updateCombo;


    public void Fire1Hold(){
        weaponMech1.OnHold();
    }
    public void Fire1Release(){
        weaponMech1.OnRelease();
    }
    public void Fire2Hold(){
        weaponMech2.OnHold();
    }
    public void Fire2Release(){
        weaponMech2.OnRelease();
    }
    public void PlaneFire1Hold(){
        weaponPlane1.OnHold();
    }
    
    //guard
    public void GuardEnter(){
        guardCollider.SetActive(true);
    }

    public void GuardExit(){
        guardCollider.GetComponent<Parry>().WillDeactivate();
    }

    //melee
    public void ChangeAttack(){
        if(meleeCombo < 2){
            meleeCombo += 1;
        }
        else{
            meleeCombo = 1;
        }
        updateCombo = false;
    }

    public void ceaseMelee(){
        meleeCombo = 0;
    }

    public void UpdateCombo(){
        updateCombo = true;
    }


    public void UpdateCombo(bool boolean){
        updateCombo = boolean;
    }

    public void InstantiateSlash(){
        if(meleeCombo >0){
            slashes[meleeCombo-1].SetActive(false);
            slashes[meleeCombo-1].SetActive(true);
            slashCollider.SetActive(false);
            slashCollider.SetActive(true);
        }

    }
}
