using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlashEnemy : MonoBehaviour
{
    [SerializeField]private float damage = 40f;
    [SerializeField]private GameObject instanciateOnHit;
    public List<GameObject> cannotDamageMore = new List<GameObject>();
    private MeleeCombat manager;
    
    void Start(){
        manager = GetComponentInParent<MeleeCombat>();
    }

    void OnEnable()
    {
        cannotDamageMore.Clear();
    }

    void OnDisable()
    {
        cannotDamageMore.Clear();
    }
    
    void InstanciateOnHit(Collider other){
        if(instanciateOnHit != null){
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Instantiate(instanciateOnHit, contactPoint, Quaternion.LookRotation(contactPoint - transform.position));
            UtilitiesStatic.Shake();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if(other.gameObject.CompareTag("PlayerGuard")){
            gameObject.SetActive(false);
            UtilitiesStatic.Shake();
            if(other.gameObject.GetComponent<Parry>().Parried()){
                manager.Parried();
                other.gameObject.GetComponent<Parry>().InstanciateOnParry(transform.position);
            }
            else{
                other.gameObject.GetComponent<Parry>().InstanciateOnGuard(transform.position);
            }
        }
        else if(!cannotDamageMore.Contains(otherGameObject)){
            cannotDamageMore.Add(otherGameObject);

            if(otherGameObject.GetComponentInChildren<Damageable>()){
                otherGameObject.GetComponentInChildren<Damageable>().takeDamage(damage, true);
                InstanciateOnHit(other);
            }
        }
    }


}
