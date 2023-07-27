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

    private bool mustDie = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(mustDie){
            Destroy(gameObject);
        }
        rb.velocity = transform.forward * speed;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed,layerMask)){
            foreach(GameObject go in instantiateOnDeath){
                Instantiate(go, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if(hit.collider.gameObject.GetComponentInChildren<NPC>()){
                hit.collider.gameObject.GetComponentInChildren<NPC>().takeDamage(damagesDealt);
            }
            mustDie = true;
        }
    }
}
