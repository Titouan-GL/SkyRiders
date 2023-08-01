using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float speed = 200f;
    [SerializeField]private LayerMask layerMask;
    [SerializeField]private List<GameObject> instantiateOnDeath;
    [SerializeField]private float damagesDealt = 5f;
    public Rigidbody rb;

    private Quaternion rotation;
    private bool mustDie = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        rotation = transform.rotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(mustDie){
            Destroy(gameObject);
        }
        rb.velocity = transform.forward * speed;
        transform.rotation = rotation;
        /*RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.fixedDeltaTime,layerMask)){
            foreach(GameObject go in instantiateOnDeath){
                Instantiate(go, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if(hit.collider.gameObject.GetComponentInChildren<Damageable>()){
                hit.collider.gameObject.GetComponentInChildren<Damageable>().takeDamage(damagesDealt);
                UtilitiesStatic.Shake();
            }
            else if(hit.collider.gameObject.GetComponentInChildren<Combat>()){
                hit.collider.gameObject.GetComponentInChildren<Combat>().takeDamage(damagesDealt, false);
            }
            mustDie = true;
        }*/
    }
    void OnTriggerEnter(Collider other){
        foreach(GameObject go in instantiateOnDeath){
            Instantiate(go, transform.position, Quaternion.identity);
        }
        if(other.gameObject.GetComponentInChildren<Damageable>()){
            other.gameObject.GetComponentInChildren<Damageable>().takeDamage(damagesDealt);
            UtilitiesStatic.Shake();
            
        }
        Destroy(gameObject);
        
    }
}
