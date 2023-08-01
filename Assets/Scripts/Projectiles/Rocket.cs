using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]private float speed = 50f;
    [SerializeField]private LayerMask layerMask;
    [SerializeField]private List<GameObject> instantiateOnDeath;
    [SerializeField]private float damagesDealt = 15f;
    [SerializeField]private float rotationSpeed = 0.05f;
    [HideInInspector] public Rigidbody rb;
    public Transform target;


    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
        if(target != null){
            Vector3 directionToTarget = target.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToTarget), rotationSpeed);
        }
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
