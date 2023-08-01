using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    [SerializeField]private float damage = 40f;
    [SerializeField]private GameObject instanciateOnHit;
    public List<GameObject> cannotDamageMore = new List<GameObject>();
    
    void OnEnable()
    {
        cannotDamageMore.Clear();
    }

    void OnDisable()
    {
        cannotDamageMore.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject otherGameObject = contact.otherCollider.gameObject;
            if(!cannotDamageMore.Contains(otherGameObject)){
                cannotDamageMore.Add(otherGameObject);

                if(otherGameObject.GetComponentInChildren<Damageable>()){
                    if(otherGameObject.GetComponentInChildren<Damageable>().IsAlive()){
                        otherGameObject.GetComponentInChildren<Damageable>().takeDamage(damage, true);
                        if(instanciateOnHit != null){
                            Instantiate(instanciateOnHit, contact.point, Quaternion.LookRotation(contact.point - transform.position));
                            UtilitiesStatic.Shake();
                        }
                    }
                }
            }
        }
    }


}
