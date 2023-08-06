using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]private float speed = 50f;
    [SerializeField]private LayerMask layerMask;
    [SerializeField]private List<GameObject> instantiateOnDeath;
    [SerializeField]private float damagesDealt = 15f;
    [SerializeField]private float rotationSpeedMax = 0.05f;
    [SerializeField]private float rotationSpeedIncreasePerSeconds = 0.2f;
    private float rotationSpeed = 0f;
    [HideInInspector] public Rigidbody rb;
    public Transform target;


    void Start(){
        rb = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotationSpeed < rotationSpeedMax){
            rotationSpeed += rotationSpeedIncreasePerSeconds * Time.deltaTime;
        }
        if(rotationSpeed > rotationSpeedMax){
            rotationSpeed = rotationSpeedMax;
        }
        rb.velocity = transform.forward * speed;
        if(target != null){
            Vector3 directionToTarget = target.position - transform.position;

            Vector3 startAngles = transform.rotation.eulerAngles;
            Vector3 endAngles = Quaternion.LookRotation(directionToTarget).eulerAngles;

            Vector3 interpolatedAngles = new Vector3(
                Mathf.LerpAngle(startAngles.x, endAngles.x, rotationSpeed),
                Mathf.LerpAngle(startAngles.y, endAngles.y, rotationSpeed),
                Mathf.LerpAngle(startAngles.z, endAngles.z, rotationSpeed)
            );

            transform.eulerAngles = interpolatedAngles;
        }
    }

    void OnTriggerEnter(Collider other){
        foreach(GameObject go in instantiateOnDeath){
            Instantiate(go, transform.position, Quaternion.identity);
        }
        if(other.gameObject.GetComponentInChildren<Damageable>()){
            other.gameObject.GetComponentInChildren<Damageable>().takeDamage(damagesDealt, true);
            UtilitiesStatic.Shake();
        }
        Destroy(gameObject);
        
    }
}
